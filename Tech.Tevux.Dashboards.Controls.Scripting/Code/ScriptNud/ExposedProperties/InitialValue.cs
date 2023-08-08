namespace Tech.Tevux.Dashboards.Controls;
public partial class ScriptNud {
    public static readonly DependencyProperty InitialValueProperty = DependencyProperty.Register(
        nameof(InitialValue),
        typeof(decimal),
        typeof(ScriptNud),
        new PropertyMetadata(0m));

    [ExposedNumber]
    [DisplayName("Initial value")]
    [Category(OptionCategory.Main)]
    public decimal InitialValue {
        get { return (decimal)GetValue(InitialValueProperty); }
        set { SetValue(InitialValueProperty, value); }
    }
}
