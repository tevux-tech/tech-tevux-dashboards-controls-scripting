using System.Collections.Generic;
using System.Runtime.Loader;

namespace Tech.Tevux.Dashboards.Controls;

public class MyLibrary : ILibrary,
                         ISharedLibraryMessengerConsumer,
                         IAssemblyContextConsumer,
                         IDashboardControlProvider,
                         IDashboardControlEditorProvider {

    private bool _isInitialized;

    private MyLibrary() { }

    public static MyLibrary Instance { get; } = new MyLibrary();
    public AssemblyLoadContext AssemblyLoadContext { get; private set; } = AssemblyLoadContext.Default;
    public Dictionary<System.Type, List<System.Type>> DashboardControlEditors { get; private set; } = null!;
    public List<System.Type> DashboardControls { get; private set; } = null!;
    public ISharedLibraryMessenger GlobalMessenger { get; set; } = new EmptyLibraryMessenger();

    #region ILibrary
    private bool _isDisposed;

    public void Dispose() {
        // A good article explaining how to implement Dispose. https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Initialize() {
        if (_isInitialized) { return; }

        DashboardControls = new List<System.Type> {
            typeof(CSharpScriptButton),
            typeof(ScriptIndicator),
            typeof(ScriptNud),
            typeof(ScriptOutput)
        };

        DashboardControlEditors = new Dictionary<System.Type, List<System.Type>>();
        DashboardControlEditors.Add(typeof(CSharpScriptButton), new List<System.Type>() { typeof(CSharpScriptEditor) });
        DashboardControlEditors.Add(typeof(ScriptIndicator), new List<System.Type>() { typeof(RulesEditor) });

        _isInitialized = true;
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

    #region ISharedLibraryMessengerConsumer

    public void SetSharedMessenger(ISharedLibraryMessenger sharedMessenger) {
        GlobalMessenger = sharedMessenger;
    }

    #endregion

    #region IAssemblyContextConsumer

    public void SetAssemblyContext(AssemblyLoadContext loadContext) {
        AssemblyLoadContext = loadContext;
    }

    #endregion

    #region IDashboardControlProvider

    public List<System.Type> GetDashboardControls() {
        return DashboardControls;
    }

    #endregion

    #region IDashboardControlEditorProvider

    public Dictionary<System.Type, List<System.Type>> GetEditors() {
        return DashboardControlEditors;
    }

    #endregion
}
