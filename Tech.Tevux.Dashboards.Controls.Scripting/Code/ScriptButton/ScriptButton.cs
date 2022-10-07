using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using DevBot9.Mvvm;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using NLog;
using Tevux.Software.Helpers;

namespace Tech.Tevux.Dashboards.Controls;

[DashboardControl]
[Category("Scripting")]
public partial class ScriptButton : ControlBase {
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    private readonly CancellationTokenSource _globalCts = new();
    private bool _isDisposed;

    public ScriptButton() {
        ExecuteCommand = new AsyncCommand(() => Task.Factory.StartNew(Execute));
    }

    public void Execute() {
        var script = "";
        var imports = "";
        var libraryContext = "";
        Dispatcher.Invoke(() => {
            script = Script;
            imports = Imports;
            libraryContext = LibraryContext;
        });

        ExecuteScript(script, imports, libraryContext, out var errorMessage);

        Dispatcher.Invoke(() => {
            ErrorMessage = errorMessage;
        });
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Official docs say you should always dispose CTS'es, but that is complicated because you then may get ObjectDisposedException.
                // Internet says it is not so crucial and Cancel() is enough in 99.9% of cases, unless one uses WaitHandles, which is very rare.
                // https://stackoverflow.com/questions/6960520/when-to-dispose-cancellationtokensource

                _globalCts.Cancel();
            }

            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }

    private void ExecuteScript(string script, string imports, string libraryContext, out string errorMessage) {
        if (LibrarySupervisor.Instance.PluginDatas.ContainsKey(libraryContext) == false) {
            errorMessage = $"Library set \"{libraryContext}\" is not loaded.";
            _log.Error(errorMessage, libraryContext);
        }
        else {
            var contextShortcut = LibrarySupervisor.Instance.PluginDatas[libraryContext].AssemblyContext;
            var scriptContextShortcut = LibrarySupervisor.Instance.PluginDatas[libraryContext].ScriptContext;
            errorMessage = "";
            _log.Info("Beginning script execution.");
            var stopwatch = Stopwatch.StartNew();

            try {
                var options = ScriptOptions.Default;

                var funkyLoader = new InteractiveAssemblyLoader();
                foreach (var assembly in contextShortcut.Assemblies) {
                    if (assembly.IsDynamic == false) {
                        _log.Info("Addding assembly {0} to script execution context.", assembly.FullName);
                        options = options.AddReferences(assembly);
                        funkyLoader.RegisterDependency(assembly);
                    }
                }

                options = options.WithImports(imports.Split("\r\n"));

                if (scriptContextShortcut is not null) {
                    _log.Info("ScriptContext has been found.");
                    var zeScript = CSharpScript.Create(script, options, scriptContextShortcut, funkyLoader);
                    zeScript.RunAsync(Activator.CreateInstance(scriptContextShortcut)).Wait();
                }
                else {
                    _log.Info("ScriptContext has not been found, continuing without it.");
                    var zeScript = CSharpScript.Create(script, options, null, funkyLoader);
                    zeScript.RunAsync().Wait();
                }
            }
            catch (TaskCanceledException) {
                // Swallowing.
            }
            catch (CompilationErrorException ex) {
                _log.Error(ex, "Script cannot compile.");
                errorMessage = ex.Message;
            }
            catch (Exception ex) {
                _log.Error(ex, "Script execution error.");
                errorMessage = ex.Message;
            }

            stopwatch.Stop();
            if (errorMessage.IsEmpty()) {
                _log.Info("Script executed succesfully in {0} s.", (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.0000", CultureInfo.InvariantCulture));
            }
        }
    }
}
