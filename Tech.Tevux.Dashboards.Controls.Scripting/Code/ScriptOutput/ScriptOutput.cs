using System.Text;
using System.Windows.Controls;
using DevBot9.Mvvm;

namespace Tech.Tevux.Dashboards.Controls;

[TemplatePart(Name = "PART_MainText", Type = typeof(TextBox))]
[HiddenExposedOption(nameof(Alignment))]
[HiddenExposedOption(nameof(Caption))]
[Category("Scripting")]
[DisplayName("Script output")]
public partial class ScriptOutput : ControlBase {
    private readonly StringBuilder _textBuilder = new();
    private bool _isDisposed;
    private TextBox? _textBox;

    static ScriptOutput() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptOutput), new FrameworkPropertyMetadata(typeof(ScriptOutput)));
    }

    public ScriptOutput() {
        HandleContextMenuClickCommand = new DelegateCommand<string>((parameter) => {
            switch (parameter) {
                case "clear":
                    _textBuilder.Clear();
                    Caption = _textBuilder.ToString();
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

    public void Reconfigure() {
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
        _textBuilder.AppendLine(message.Value.ToString());
        if (_textBuilder.Length > 8000) {
            _textBuilder.Remove(0, _textBuilder.Length - 8000);
        }
        Dispatcher.Invoke(() => {
            Caption = _textBuilder.ToString();
            _textBox?.ScrollToEnd();
        });
    }
}
