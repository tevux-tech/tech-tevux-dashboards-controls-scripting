using System.Collections.ObjectModel;

namespace Tech.Tevux.Dashboards.Controls;

public partial class ScriptEditor {
    public static readonly DependencyProperty AvailableContextsProperty = DependencyProperty.Register(
        nameof(AvailableContexts),
        typeof(ObservableCollection<LibraryComboBoxItem>),
        typeof(ScriptEditor),
        new PropertyMetadata(null));

    public ObservableCollection<LibraryComboBoxItem> AvailableContexts {
        get { return (ObservableCollection<LibraryComboBoxItem>)GetValue(AvailableContextsProperty); }
        set { SetValue(AvailableContextsProperty, value); }
    }
}
