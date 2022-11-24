using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Formatting;

namespace Tech.Tevux.Dashboards.Controls;

public class LibrarySupervisor : ISharedLibraryMessengerInitializer {
    private bool _isInitialized;

    private LibrarySupervisor() {
        GlobalMessenger = new EmptyPluginMessenger();
    }

    public static LibrarySupervisor Instance { get; } = new LibrarySupervisor();
    public Dictionary<string, LibraryData> PluginDatas { get; } = new();
    public ISharedLibraryMessenger GlobalMessenger { get; set; }

    public void Initialize(ISharedLibraryMessenger globalMessenger) {
        if (_isInitialized) { return; }

        GlobalMessenger = globalMessenger;

        GlobalMessenger.Register<LibraryDataChangedMessage>(this, UpdatePluginData);

        _isInitialized = true;
    }
    internal void UpdatePluginData(LibraryDataChangedMessage message) {
        PluginDatas.Clear();
        foreach (var context in message.AvailableLibraryData) {
            if (context.Value.ScriptContext is not null) {
                PluginDatas.Add(context.Key, context.Value);
            }
        }

        // Running a tiny script in the background. This will load C# scripting libraries into memory,
        // thus there will be no frozen UI when user executes a script for the first time.
        new Thread(() => {
            var script = "var bybis=1;";
            var workspace = new AdhocWorkspace();
            var node = CSharpSyntaxTree.ParseText(script).GetRoot();
            Formatter.Format(node, workspace);
            CSharpScript.EvaluateAsync(script).Wait();
        }).Start();
    }
}
