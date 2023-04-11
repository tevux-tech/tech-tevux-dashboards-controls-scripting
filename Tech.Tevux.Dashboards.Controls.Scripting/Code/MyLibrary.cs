using System.Collections.Generic;
using System.Runtime.Loader;

namespace Tech.Tevux.Dashboards.Controls;

public class MyLibrary : ILibrary,
                         IDashboardControlProvider,
                         IDashboardControlEditorProvider {

    private bool _isInitialized;

    private MyLibrary() {
        DashboardControls.Add(typeof(CSharpScriptButton));
        DashboardControls.Add(typeof(ScriptIndicator));
        DashboardControls.Add(typeof(ScriptNud));
        DashboardControls.Add(typeof(ScriptOutput));

        DashboardControlEditors.Add(typeof(CSharpScriptButton), new List<System.Type>() { typeof(CSharpScriptEditor) });
        DashboardControlEditors.Add(typeof(ScriptIndicator), new List<System.Type>() { typeof(RulesEditor) });
    }

    public static MyLibrary Instance { get; } = new MyLibrary();

    #region Dependency injection

    [InjectedByHost]
    public AssemblyLoadContext AssemblyLoadContext { get; set; } = AssemblyLoadContext.Default;

    [InjectedByHost]
    public ISharedLibraryMessagingProvider GlobalMessenger { get; set; } = new EmptySharedLibraryMessagingProvider();

    #endregion

    #region Dependency providers 

    public Dictionary<System.Type, List<System.Type>> DashboardControlEditors { get; private set; } = new();
    public List<System.Type> DashboardControls { get; private set; } = new();

    #endregion

    #region ILibrary
    public void Initialize() {
        if (_isInitialized) { return; }

        _isInitialized = true;
    }

    #endregion

    #region IDisposable

    private bool _isDisposed;

    public void Dispose() {
        // A good article explaining how to implement Dispose. https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Dispose managed objects here.
            }

            // Free unmanaged resources here and set large fields to null.

            _isDisposed = true;
        }
    }
    #endregion
}
