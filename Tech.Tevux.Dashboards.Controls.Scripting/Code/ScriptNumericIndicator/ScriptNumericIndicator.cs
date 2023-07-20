namespace Tech.Tevux.Dashboards.Controls;

[Category("General")]
[DisplayName("Script numeric indicator")]
public partial class ScriptNumericIndicator : NumericOutputControlBase {
    private readonly ISharedLibraryMessagingProvider _interLibraryMessenger;
    private bool _isDisposed;

    static ScriptNumericIndicator() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptNumericIndicator), new FrameworkPropertyMetadata(typeof(ScriptNumericIndicator)));
    }

    public ScriptNumericIndicator() {
        _interLibraryMessenger = ScriptingLibrary.Instance.GlobalMessenger;
    }
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(this)) { return; }

        Reconfigure();
    }

    public override void Reconfigure() {
        base.Reconfigure();

        _interLibraryMessenger.Unregister(this);
        _interLibraryMessenger.Register<SetValueMessage>(this, Id, HandleSetValueMessage);
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Free managed resources here.
                _interLibraryMessenger.Unregister(this);
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
