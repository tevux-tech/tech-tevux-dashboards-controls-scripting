namespace Tech.Tevux.Dashboards.Controls;

[Category("General")]
[DisplayName("Script numeric indicator")]
public partial class ScriptNumericIndicator : NumericOutputControlBase {
    private bool _isDisposed;

    static ScriptNumericIndicator() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptNumericIndicator), new FrameworkPropertyMetadata(typeof(ScriptNumericIndicator)));
    }

    public ScriptNumericIndicator() {
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
            }

            // Free unmanaged resources here and set large fields to null.
            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }

    private void HandleSetValueMessage(SetValueMessage message) {
        Dispatcher.Invoke(() => {
            if (AutoConverter.TryGetAsNumber(message.Value, out var number)) {
                NumericValue = number;
            } else {
                ErrorMessage = "Value sent to this control is not a number.";
            }
        });
    }
}
