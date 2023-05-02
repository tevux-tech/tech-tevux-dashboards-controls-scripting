namespace Tech.Tevux.Dashboards.Controls;

[Category("General")]
[DisplayName("Script textual indicator")]
public partial class ScriptTextualIndicator : TextualOutputControlBase {
    private bool _isDisposed;

    static ScriptTextualIndicator() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptTextualIndicator), new FrameworkPropertyMetadata(typeof(ScriptTextualIndicator)));
    }

    public ScriptTextualIndicator() {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) { return; }
    }
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(this)) { return; }

        Reconfigure();
    }

    public override void Reconfigure() {
        base.Reconfigure();

        MyLibrary.Instance.GlobalMessenger.Unregister(this);
        MyLibrary.Instance.GlobalMessenger.Register<SetValueMessage>(this, Id, HandleSetValueMessage);
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Free managed resources here.
                MyLibrary.Instance.GlobalMessenger.Unregister(this);
            }

            // Free unmanaged resources here and set large fields to null.
            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }

    private void HandleSetValueMessage(SetValueMessage message) {
        Dispatcher.Invoke(() => {
            if (AutoConverter.TryGetAsText(message.Value, out var text)) {
                TextualValue = text;
            } else {
                ErrorMessage = "Value sent to this control is not a text.";
            }
        });
    }
}
