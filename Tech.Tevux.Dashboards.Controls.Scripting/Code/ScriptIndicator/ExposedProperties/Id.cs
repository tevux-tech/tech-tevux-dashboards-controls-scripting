namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptIndicator {
    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id),
        typeof(string),
        typeof(ScriptIndicator),
        new PropertyMetadata("noname", (obj, e) => {
            ((ScriptIndicator)obj).Reconfigure();
        }));

    [ExposedOption(OptionType.SingleLineText)]
    [Category(OptionCategory.Main)]
    public string Id {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
}
