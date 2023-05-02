namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptTextualIndicator {
    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id),
        typeof(string),
        typeof(ScriptTextualIndicator),
        new PropertyMetadata("noname", (d, e) => {
            ((ScriptTextualIndicator)d).Reconfigure();
        }));

    [ExposedOption(OptionType.SingleLineText)]
    [Category(OptionCategory.Main)]
    public string Id {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
}
