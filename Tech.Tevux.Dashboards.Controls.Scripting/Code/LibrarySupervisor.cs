
namespace Tech.Tevux.Dashboards.Controls;

public class LibrarySupervisor : ISharedLibraryMessengerInitializer {
    private bool _isInitialized;

    private LibrarySupervisor() {
        GlobalMessenger = new EmptyPluginMessenger();
    }

    public static LibrarySupervisor Instance { get; } = new LibrarySupervisor();

    public ISharedLibraryMessenger GlobalMessenger { get; set; }

    public void Initialize(ISharedLibraryMessenger sharedMessenger) {
        if (_isInitialized) { return; }

        GlobalMessenger = sharedMessenger;

        _isInitialized = true;
    }
}
