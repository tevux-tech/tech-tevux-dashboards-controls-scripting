
using System.Runtime.Loader;

namespace Tech.Tevux.Dashboards.Controls;

public class LibrarySupervisor : ISharedLibraryMessengerConsumer, IAssemblyContextConsumer {
    private bool _isInitialized;

    private LibrarySupervisor() {
        GlobalMessenger = new EmptyLibraryMessenger();
    }

    public static LibrarySupervisor Instance { get; } = new LibrarySupervisor();
    public AssemblyLoadContext AssemblyLoadContext { get; private set; } = AssemblyLoadContext.Default;
    public ISharedLibraryMessenger GlobalMessenger { get; set; }

    public void SetAssemblyContext(AssemblyLoadContext loadContext) {
        AssemblyLoadContext = loadContext;
    }

    public void SetSharedMessenger(ISharedLibraryMessenger sharedMessenger) {
        if (_isInitialized) { return; }

        GlobalMessenger = sharedMessenger;

        _isInitialized = true;
    }
}
