namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptEditor {
    public static readonly DependencyProperty LocalScriptTextProperty = DependencyProperty.Register(
        nameof(LocalScriptText),
        typeof(string),
        typeof(ScriptEditor),
        new PropertyMetadata("", (d, e) => {
            (d as ScriptEditor)?.HandleScriptChange(e.NewValue);
        }));

    public string LocalScriptText {
        get { return (string)GetValue(LocalScriptTextProperty); }
        set { SetValue(LocalScriptTextProperty, value); }
    }
}
