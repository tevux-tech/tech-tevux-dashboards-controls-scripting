namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptOutput {
    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id),
        typeof(string),
        typeof(ScriptOutput),
        new PropertyMetadata("debug-output", (obj, e) => {
            ((ScriptOutput)obj).Reconfigure();
        }));

    [ExposedSingleLineText]
    [Category(OptionCategory.Main)]
    public string Id {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
}
