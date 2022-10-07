namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptEditor {
    public static readonly DependencyProperty LocalSelectedContextProperty = DependencyProperty.Register(
        nameof(LocalSelectedContext),
        typeof(string),
        typeof(ScriptEditor),
        new PropertyMetadata(""));

    public string LocalSelectedContext {
        get { return (string)GetValue(LocalSelectedContextProperty); }
        set { SetValue(LocalSelectedContextProperty, value); }
    }
}
