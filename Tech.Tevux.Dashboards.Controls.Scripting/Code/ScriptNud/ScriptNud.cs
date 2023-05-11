using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace Tech.Tevux.Dashboards.Controls;

[HideExposedOption(nameof(Caption))]
[HideExposedOption(nameof(Alignment))]
[Category("General")]
[DisplayName("Script numeric up-down")]
public partial class ScriptNud : NumericInputControlBase {
    private bool _isDisposed;
    private Control? _theNud;

    static ScriptNud() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptNud), new FrameworkPropertyMetadata(typeof(ScriptNud)));
    }

    public ScriptNud() { }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(this)) { return; }

        NumericValue = InitialValue;

        if (Template.FindName("PART_MainGrid", this) is Grid grid) {
            BindingOperations.SetBinding(grid, Grid.ToolTipProperty, new Binding(nameof(TooltipText)) { Source = this });

            var nud = new NumericUpDown() {
                MinWidth = 10,
                HideUpDownButtons = true,
                TextAlignment = TextAlignment.Right,
                Background = Brushes.White
            };

            _theNud = nud;

            BindingOperations.SetBinding(nud, NumericUpDown.ValueProperty, new Binding(nameof(NumericValue)) { Source = this, Mode = BindingMode.TwoWay });

            grid.Children.Add(nud);
        }

        Reconfigure();
    }

    public override void Reconfigure() {
        base.Reconfigure();

        if (_theNud is NumericUpDown theRealNud) {
            theRealNud.FontSize = TextSize;
            theRealNud.Interval = (double)Step;
            theRealNud.Minimum = (double)Minimum;
            theRealNud.Maximum = (double)Maximum;
            theRealNud.StringFormat = "F" + DecimalPlaces;
        }

        MyLibrary.Instance.GlobalMessenger.Unregister(this);
        MyLibrary.Instance.GlobalMessenger.Register<GetValueMessage>(this, Id, HandleGetValueMessage);
    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Official docs say you should always dispose CTS'es, but that is complicated because you then may get ObjectDisposedException.
                // Internet says it is not so crucial and Cancel() is enough in 99.9% of cases, unless one uses WaitHandles, which is very rare.
                // https://stackoverflow.com/questions/6960520/when-to-dispose-cancellationtokensource

                // _globalCts.Cancel();
                MyLibrary.Instance.GlobalMessenger.Unregister(this);
            }

            _isDisposed = true;
        }

        base.Dispose(isCalledManually);
    }

    private void HandleGetValueMessage(GetValueMessage message) {
        Dispatcher.Invoke(() => {
            message.Value = NumericValue;
            message.IsFinished = true;
        });
    }
}
