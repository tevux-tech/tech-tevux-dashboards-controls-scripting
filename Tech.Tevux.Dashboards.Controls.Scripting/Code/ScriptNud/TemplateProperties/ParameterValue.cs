namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptNud {
    public static readonly DependencyProperty ParameterValueProperty = DependencyProperty.Register(
        nameof(ParameterValue),
        typeof(decimal),
        typeof(ScriptNud),
        new PropertyMetadata(0m));
    public decimal ParameterValue {
        get { return (decimal)GetValue(ParameterValueProperty); }
        set { SetValue(ParameterValueProperty, value); }
    }
}
