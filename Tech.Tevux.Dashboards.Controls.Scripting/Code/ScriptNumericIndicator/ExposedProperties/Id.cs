namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptNumericIndicator {
    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id),
        typeof(string),
        typeof(ScriptNumericIndicator),
        new PropertyMetadata("noname", (d, e) => {
            ((ScriptNumericIndicator)d).Reconfigure();
        }));

    [ExposedSingleLineText]
    [Category(OptionCategory.Main)]
    public string Id {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
}
