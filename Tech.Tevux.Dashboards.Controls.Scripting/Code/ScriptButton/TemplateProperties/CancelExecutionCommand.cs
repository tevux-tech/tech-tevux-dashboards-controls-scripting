namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptButton {
    public static readonly DependencyProperty CancelExecutionProperty = DependencyProperty.Register(
        nameof(CancelExecutionCommand),
        typeof(ICommand),
        typeof(ScriptButton),
        new PropertyMetadata(null));

    public ICommand CancelExecutionCommand {
        get { return (ICommand)GetValue(CancelExecutionProperty); }
        set { SetValue(CancelExecutionProperty, value); }
    }
}
