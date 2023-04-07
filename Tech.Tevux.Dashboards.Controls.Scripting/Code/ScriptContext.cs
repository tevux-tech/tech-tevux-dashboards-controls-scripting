namespace Tech.Tevux.Dashboards.Controls;

public class ScriptContext : ScriptContextBase {
    public override ISharedLibraryMessagingProvider Messenger { get; } = MyLibrary.Instance.GlobalMessenger!;
}
