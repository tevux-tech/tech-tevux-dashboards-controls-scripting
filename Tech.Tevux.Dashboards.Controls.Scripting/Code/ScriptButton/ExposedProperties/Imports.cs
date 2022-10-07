namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptButton {
    public static readonly DependencyProperty ImportsProperty = DependencyProperty.Register(
        nameof(Imports),
        typeof(string),
        typeof(ScriptButton),
        new PropertyMetadata("System\r\nSystem.Threading\r\nTevux.Dashboards.Abstractions\r\nSystem.Diagnostics"));

    [ExposedOption(OptionType.MultiLineText)]
    [Category(OptionCategory.Main)]
    public string Imports {
        get { return (string)GetValue(ImportsProperty); }
        set { SetValue(ImportsProperty, value); }
    }
}
