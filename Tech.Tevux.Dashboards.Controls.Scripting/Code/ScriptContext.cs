namespace Tech.Tevux.Dashboards.Controls;

public class ScriptContext : ScriptContextBase {
    public override ISharedLibraryMessenger Messenger { get; } = LibrarySupervisor.Instance.GlobalMessenger!;
}
