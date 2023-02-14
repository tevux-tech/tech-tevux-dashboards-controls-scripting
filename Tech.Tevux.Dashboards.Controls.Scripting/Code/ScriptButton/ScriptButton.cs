using System.Threading.Tasks;
using DevBot9.Mvvm;
using NLog;

namespace Tech.Tevux.Dashboards.Controls;

[DashboardControl]
[Category("Scripting")]
public partial class ScriptButton : ControlBase {
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    private readonly CancellationTokenSource _globalCts = new();
    private ExecuteScriptMessage _executeScriptMessage = new();
    private bool _isDisposed;

    public ScriptButton() {
        ExecuteCommand = new AsyncCommand(() => Task.Factory.StartNew(Execute));
        CancelExecutionCommand = new AsyncCommand(() => Task.Factory.StartNew(CancelExecution), CanCancelExecutionCommand);
    }

    public bool CanCancelExecutionCommand() {
        return ((AsyncCommand)ExecuteCommand).IsExecuting;
    }
    public void CancelExecution() {
        if (_executeScriptMessage.IsFinished == false) {
            if (_executeScriptMessage.FeedbackObject is ScriptContextBase context) {
                _log.Info("Attempting to stop the script.");
                context.IsCancellationRequested = true;
            }
        }
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

        _executeScriptMessage = new ExecuteScriptMessage();
        _executeScriptMessage.Arguments["script"] = script;
        _executeScriptMessage.Arguments["imports"] = imports;
        _executeScriptMessage.Arguments["libraryContext"] = libraryContext ?? "";

        Task.Run(async () => {
            _log.Info("Asking to run the script");
            LibrarySupervisor.Instance.GlobalMessenger.Send(_executeScriptMessage);
            while (_executeScriptMessage.IsFinished == false) {
                await Task.Delay(100);
            }
            _log.Info("Script execution finished.");
        }).Wait();

        Dispatcher.Invoke((Delegate)(() => {
            ErrorMessage = _executeScriptMessage.ErrorMessage;
        }));
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
                CancelExecution();
            }

            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }
}
