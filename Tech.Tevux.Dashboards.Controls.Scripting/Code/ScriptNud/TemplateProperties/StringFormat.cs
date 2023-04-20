namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptNud {
    public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
        nameof(StringFormat),
        typeof(string),
        typeof(ScriptNud),
        new PropertyMetadata("{0}"));

    public string StringFormat {
        get { return (string)GetValue(StringFormatProperty); }
        set { SetValue(StringFormatProperty, value); }
    }
}
