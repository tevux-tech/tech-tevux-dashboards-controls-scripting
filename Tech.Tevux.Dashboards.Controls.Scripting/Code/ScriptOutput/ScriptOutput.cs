using System.Text;
using System.Windows.Controls;
using DevBot9.Mvvm;

namespace Tech.Tevux.Dashboards.Controls;

[HideExposedOption(nameof(Alignment))]
[HideExposedOption(nameof(Caption))]
[Category("General")]
[DisplayName("Script output")]
public partial class ScriptOutput : ControlBase {
    private readonly ISharedLibraryMessagingProvider _interLibraryMessenger;
    private readonly StringBuilder _textBuilder = new();
    private bool _isDisposed;
    private TextBox? _textBox;

    static ScriptOutput() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptOutput), new FrameworkPropertyMetadata(typeof(ScriptOutput)));
    }

    public ScriptOutput() {
        _interLibraryMessenger = MyLibrary.Instance.GlobalMessenger;

        HandleContextMenuClickCommand = new DelegateCommand<string>((parameter) => {
            switch (parameter) {
                case "clear":
                    _textBuilder.Clear();
                    OutputText = _textBuilder.ToString();
                    break;
            }
        });
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) { return; }

        if (Template.FindName("PART_MainText", this) is TextBox textbox) {
            _textBox = textbox;
        }

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
        _textBuilder.AppendLine(message.Value.ToString());
        if (_textBuilder.Length > 8000) {
            _textBuilder.Remove(0, _textBuilder.Length - 8000);
        }
        Dispatcher.Invoke(() => {
            OutputText = _textBuilder.ToString();
            _textBox?.ScrollToEnd();
        });
    }
}
