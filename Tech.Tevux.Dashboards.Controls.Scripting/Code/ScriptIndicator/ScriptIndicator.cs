namespace Tech.Tevux.Dashboards.Controls;

[Category("Scripting")]
[DisplayName("Script indicator")]
public partial class ScriptIndicator : OutputControlBase {
    private bool _isDisposed;

    static ScriptIndicator() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptIndicator), new FrameworkPropertyMetadata(typeof(ScriptIndicator)));
    }

    public ScriptIndicator() {
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
                ApplyAppearanceRules(number);
            } else if (AutoConverter.TryGetAsText(message.Value, out var text)) {
                ApplyAppearanceRules(text);
            }
        });
    }
}
