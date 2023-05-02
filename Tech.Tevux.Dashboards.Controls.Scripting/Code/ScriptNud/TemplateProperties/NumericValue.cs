namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptNud {
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(NumericValue),
        typeof(decimal),
        typeof(ScriptNud),
        new PropertyMetadata(0m));

    [DisplayName("Value")]
    [Category("Main")]
    public decimal NumericValue {
        get { return (decimal)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
}
