using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DevBot9.Mvvm;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;

namespace Tech.Tevux.Dashboards.Controls;

[OptionEditor(typeof(ScriptButton), "Script editor")]
public partial class ScriptEditor : Control, IDisposable {
    private readonly CancellationTokenSource _globalCts = new();
    private readonly ScriptButton? _scriptButton;
    private bool _isChangeOriginatingFromEditor;
    private bool _isDisposed = false;
    private TextEditor? _textEditor;

    static ScriptEditor() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScriptEditor), new FrameworkPropertyMetadata(typeof(ScriptEditor)));
    }

    public ScriptEditor() {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) { return; }
    }

    public ScriptEditor(ScriptButton control) {
        _scriptButton = control;

        ExecuteGuiCommand = new DelegateCommand<string>(argument => {
            switch (argument) {
                case "format":
                    var workspace = new AdhocWorkspace();

                    var options = workspace.Options;
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, false);
                    options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, false);

                    var bybis = new CSharpParseOptions();


                    var node = CSharpSyntaxTree.ParseText(LocalScriptText, bybis).GetRoot();
                    var formatResult = Formatter.Format(node, workspace, options);

                    LocalScriptText = formatResult.ToString();
                    break;

            }
        }, argument => {
            switch (argument) {
                case "format":
                    return true;

                default:
                    return false;
            }
        });

        // Binding to control properties for synchronisation purposes.
        var scriptBinding = new Binding(nameof(ScriptButton.Script)) { Source = _scriptButton, Mode = BindingMode.TwoWay };
        scriptBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        BindingOperations.SetBinding(this, LocalScriptTextProperty, scriptBinding);

        var contextBinding = new Binding(nameof(ScriptButton.LibraryContext)) { Source = _scriptButton, Mode = BindingMode.TwoWay };
        contextBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        BindingOperations.SetBinding(this, LocalSelectedContextProperty, contextBinding);
    }

    public List<Style> AllStyles { get; private set; } = Controls.Style.GetAllStyles();

    public void Dispose() {
        // A good article explaining how to implement Dispose. https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    public void HandleScriptChange(object newValue) {
        if (_isDisposed) { return; }

        var newText = (string)newValue;

        if (_isChangeOriginatingFromEditor == false) {
            if (_textEditor != null) { _textEditor.Text = newText; }
        }

        _isChangeOriginatingFromEditor = false;
    }

    public record class LibraryComboBoxItem(string Key, string FriendlyName);
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (Template.FindName("PART_MainGrid", this) is Grid dataGrid) {
            _textEditor = new TextEditor();
            _textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            _textEditor.FontFamily = new FontFamily("Courier New");
            _textEditor.ShowLineNumbers = true;
            _textEditor.TextChanged += HandleTextChangedEvent;
            _textEditor.Text = (string)GetValue(LocalScriptTextProperty);

            dataGrid.Children.Add(_textEditor);
        }
    }

    protected virtual void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                _globalCts.Cancel();
                if (_textEditor != null) { _textEditor.TextChanged -= HandleTextChangedEvent; }

            }

            // Free unmanaged resources here and set large fields to null.
            _isDisposed = true;
        }
    }

    private void HandleTextChangedEvent(object? sender, EventArgs e) {
        _isChangeOriginatingFromEditor = true;
        LocalScriptText = _textEditor!.Text;
    }
}
