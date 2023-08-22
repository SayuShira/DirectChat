using Dalamud.Game.Command;
using Dalamud.Game.Config;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace DirectChat;

public class DirectChat : IDalamudPlugin
{
    public string Name => "DirectChat";

    private CommandManager CommandManager { get; init; }
    private GameConfig GameConfig { get; }
    private ChatGui Chat { get; }

    private const string CommandName = "/directchat";

    public DirectChat(
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] GameConfig gameConfig,
        [RequiredVersion("1.0")] ChatGui chat)
    {
        CommandManager = commandManager;
        GameConfig = gameConfig;
        Chat = chat;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) {
            HelpMessage = "Toggle the games Direct Chat"
        });
    }

    private void OnCommand(string command, string args)
    {
        GameConfig.UiControl.TryGet("DirectChat", out bool directChat);
        PluginLog.Debug($"Is Direct Chat on? {directChat.ToString()}");

        GameConfig.UiControl.Set("DirectChat", !directChat);

        // The initial bool is always the opposite value
        var status = !directChat ? "active" : "disabled";

        Chat.Print($"DirectChat is now {status}.");
    }

    public void Dispose()
    {
        CommandManager.RemoveHandler(CommandName);
    }
}
