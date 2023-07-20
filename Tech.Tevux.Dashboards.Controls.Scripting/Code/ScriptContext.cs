namespace Tech.Tevux.Dashboards.Controls;

public class ScriptContext : ScriptContextBase {
    public override ISharedLibraryMessagingProvider Messenger { get; } = ScriptingLibrary.Instance.GlobalMessenger!;
}
