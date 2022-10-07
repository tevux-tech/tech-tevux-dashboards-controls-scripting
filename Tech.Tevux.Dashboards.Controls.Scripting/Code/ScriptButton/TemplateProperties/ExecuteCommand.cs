namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptButton {
    public static readonly DependencyProperty ExecuteCommandProperty = DependencyProperty.Register(
        nameof(ExecuteCommand),
        typeof(ICommand),
        typeof(ScriptButton),
        new PropertyMetadata(null));

    public ICommand ExecuteCommand {
        get { return (ICommand)GetValue(ExecuteCommandProperty); }
        set { SetValue(ExecuteCommandProperty, value); }
    }
}
