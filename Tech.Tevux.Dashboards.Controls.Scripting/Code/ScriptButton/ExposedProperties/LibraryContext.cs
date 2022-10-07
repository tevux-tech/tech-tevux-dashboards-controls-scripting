namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptButton {
    public static readonly DependencyProperty LibraryContextProperty = DependencyProperty.Register(
        nameof(LibraryContext),
        typeof(string),
        typeof(ScriptButton),
        new PropertyMetadata("Tech.Tevux.Dashboards.Controls.Scripting"));

    [ExposedOption(OptionType.SingleLineText)]
    [DisplayName("Library context")]
    [Category(OptionCategory.Main)]
    public string LibraryContext {
        get { return (string)GetValue(LibraryContextProperty); }
        set { SetValue(LibraryContextProperty, value); }
    }
}
