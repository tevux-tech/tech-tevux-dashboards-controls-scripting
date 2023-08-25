namespace Tech.Tevux.Dashboards.Controls;

[Category("General")]
[DisplayName("Script button (C#)")]
public partial class CSharpScriptButton : CSharpScriptControlBase {
    private readonly CancellationTokenSource _globalCts = new();
    private bool _isDisposed;

    static CSharpScriptButton() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CSharpScriptButton), new FrameworkPropertyMetadata(typeof(CSharpScriptButton)));
    }

    public CSharpScriptButton() {

    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        AssemblyLoadContext = ScriptingLibrary.Instance.AssemblyLoadContext;
        ScriptContext = new ScriptContext();
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
