namespace Tech.Tevux.Dashboards.Controls;

public class ScriptContext : ScriptContextBase
{
    public override ISharedLibraryMessenger Messenger { get; } = MyLibrary.Instance.GlobalMessenger!;
}
