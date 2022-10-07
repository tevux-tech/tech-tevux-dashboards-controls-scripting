namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptNud {
    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id),
        typeof(string),
        typeof(ScriptNud),
        new PropertyMetadata("noname", (obj, e) => {
            ((ScriptNud)obj).Reconfigure();
        }));

    [ExposedOption(OptionType.SingleLineText)]
    [Category(OptionCategory.Main)]
    public string Id {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
}
