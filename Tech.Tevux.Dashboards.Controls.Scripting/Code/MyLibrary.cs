using System.Collections.Generic;
using System.Runtime.Loader;

namespace Tech.Tevux.Dashboards.Controls;

public class MyLibrary : ISharedLibraryMessengerConsumer,
                         IAssemblyContextConsumer,
                         IDashboardControlProvider,
                         IDashboardControlEditorProvider {
    private bool _isInitialized;

    private MyLibrary() {
        GlobalMessenger = new EmptyLibraryMessenger();
    }

    public static MyLibrary Instance { get; } = new MyLibrary();
    public AssemblyLoadContext AssemblyLoadContext { get; private set; } = AssemblyLoadContext.Default;
    public ISharedLibraryMessenger GlobalMessenger { get; set; } = new EmptyLibraryMessenger();

    private void Initialize() {
        if (_isInitialized) { return; }



        _isInitialized = true;
    }

    #region ISharedLibraryMessengerConsumer

    public void SetSharedMessenger(ISharedLibraryMessenger sharedMessenger) {
        GlobalMessenger = sharedMessenger;

        GlobalMessenger.Register(this, (LoadingCompleteMessage message) => {
            Initialize();
        });
    }

    #endregion

    #region IAssemblyContextConsumer

    public void SetAssemblyContext(AssemblyLoadContext loadContext) {
        AssemblyLoadContext = loadContext;
    }

    #endregion

    #region IDashboardControlProvider

    public List<System.Type> GetDashboardControls() {
        var controlList = new List<System.Type> {
            typeof(CSharpScriptButton),
            typeof(ScriptIndicator),
            typeof(ScriptNud),
            typeof(ScriptOutput)
        };

        return controlList;
    }

    #endregion

    #region IDashboardControlEditorProvider

    public Dictionary<System.Type, List<System.Type>> GetEditors() {
        var editors = new Dictionary<System.Type, List<System.Type>>();
        editors.Add(typeof(CSharpScriptButton), new List<System.Type>() { typeof(CSharpScriptEditor) });

        return editors;
    }

    #endregion
}
