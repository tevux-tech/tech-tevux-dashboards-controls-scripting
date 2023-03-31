using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace Tech.Tevux.Dashboards.Controls;

[HiddenExposedOption(nameof(Caption))]
[HiddenExposedOption(nameof(Rules))]
[HiddenExposedOption(nameof(Alignment))]
[Category("Scripting")]
[DisplayName("Script numeric up-down")]
public partial class ScriptNud : InputOutputControlBase {
    private bool _isDisposed;

    public ScriptNud() { }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(this)) { return; }

        if (Template.FindName("PART_MainGrid", this) is Grid grid) {
            BindingOperations.SetBinding(grid, Grid.ToolTipProperty, new Binding(nameof(TooltipText)) { Source = this });

            var nud = new NumericUpDown() {
                MinWidth = 10,
                HideUpDownButtons = true,
                TextAlignment = TextAlignment.Right,
                Background = Brushes.White
            };

            BindingOperations.SetBinding(nud, NumericUpDown.FontSizeProperty, new Binding(nameof(TextSize)) { Source = this });
            BindingOperations.SetBinding(nud, NumericUpDown.IntervalProperty, new Binding(nameof(Step)) { Source = this });
            BindingOperations.SetBinding(nud, NumericUpDown.MinimumProperty, new Binding(nameof(Minimum)) { Source = this });
            BindingOperations.SetBinding(nud, NumericUpDown.MaximumProperty, new Binding(nameof(Maximum)) { Source = this });
            BindingOperations.SetBinding(nud, NumericUpDown.StringFormatProperty, new Binding(nameof(CombinedFormat)) { Source = this });
            BindingOperations.SetBinding(nud, NumericUpDown.ValueProperty, new Binding(nameof(ParameterValue)) { Source = this, Mode = BindingMode.TwoWay });

            grid.Children.Add(nud);
        }

        Reconfigure();
    }

    public override void Reconfigure() {
        base.Reconfigure();

        MyLibrary.Instance.GlobalMessenger.Unregister(this);
        MyLibrary.Instance.GlobalMessenger.Register<SetValueMessage>(this, Id, HandleSetValueMessage);
        MyLibrary.Instance.GlobalMessenger.Register<GetValueMessage>(this, Id, HandleGetValueMessage);
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Official docs say you should always dispose CTS'es, but that is complicated because you then may get ObjectDisposedException.
                // Internet says it is not so crucial and Cancel() is enough in 99.9% of cases, unless one uses WaitHandles, which is very rare.
                // https://stackoverflow.com/questions/6960520/when-to-dispose-cancellationtokensource

                // _globalCts.Cancel();

            }

            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }

    private void HandleGetValueMessage(GetValueMessage message) {
        Dispatcher.Invoke(() => {
            message.Value = ParameterValue;
            message.IsFinished = true;
        });
    }

    private void HandleSetValueMessage(SetValueMessage message) {
        Dispatcher.Invoke(() => {
            if (AutoConverter.TryGetAsNumber(message.Value, out var number)) {
                ApplyAppearanceRules(number);
            }
        });
    }
}
