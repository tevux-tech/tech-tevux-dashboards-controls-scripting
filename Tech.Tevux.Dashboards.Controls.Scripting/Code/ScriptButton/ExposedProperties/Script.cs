namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptButton {
    public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register(
        nameof(Script),
        typeof(string),
        typeof(ScriptButton),
        new PropertyMetadata("// For this to work, add a ScriptOutput control to Canvas \r\nMessenger.SetValue(\"debug-output\", \"Hello world!\");"));

    [ExposedOption(OptionType.MultiLineText)]
    public string Script {
        get { return (string)GetValue(ScriptProperty); }
        set { SetValue(ScriptProperty, value); }
    }
}
