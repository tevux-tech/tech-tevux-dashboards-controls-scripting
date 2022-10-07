namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptOutput {
    public static readonly DependencyProperty HandleContextMenuClickCommandProperty = DependencyProperty.Register(
        nameof(HandleContextMenuClickCommand),
        typeof(ICommand),
        typeof(ScriptOutput),
        new PropertyMetadata(null));

    public ICommand HandleContextMenuClickCommand {
        get { return (ICommand)GetValue(HandleContextMenuClickCommandProperty); }
        set { SetValue(HandleContextMenuClickCommandProperty, value); }
    }
}
