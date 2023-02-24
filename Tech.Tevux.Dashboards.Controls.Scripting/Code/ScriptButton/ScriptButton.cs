using System.Threading.Tasks;
using DevBot9.Mvvm;
using NLog;

namespace Tech.Tevux.Dashboards.Controls;

[DashboardControl]
[Category("Scripting")]
public partial class CSharpScriptButton : CSharpScriptControlBase {
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    private readonly CancellationTokenSource _globalCts = new();
    private bool _isDisposed;

    public CSharpScriptButton() {
        ExecuteCommand = new AsyncCommand(() => Task.Factory.StartNew(Execute));
        CancelExecutionCommand = new AsyncCommand(() => Task.Factory.StartNew(CancelExecution), CanCancelExecutionCommand);
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        AssemblyLoadContext = LibrarySupervisor.Instance.AssemblyLoadContext;
        ScriptContext = (ScriptContextBase)Activator.CreateInstance(typeof(ScriptContext))!;
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Official docs say you should always dispose CTS'es, but that is complicated because you then may get ObjectDisposedException.
                // Internet says it is not so crucial and Cancel() is enough in 99.9% of cases, unless one uses WaitHandles, which is very rare.
                // https://stackoverflow.com/questions/6960520/when-to-dispose-cancellationtokensource

                _globalCts.Cancel();
                CancelExecution();
            }

            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }
}
