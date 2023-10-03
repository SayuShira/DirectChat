using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace DirectChat;

public class DirectChat : IDalamudPlugin
{
    public string Name => "DirectChat";

    private ICommandManager CommandManager { get; init; }
    private IGameConfig GameConfig { get; }
    private IChatGui Chat { get; }
    private IPluginLog PluginLog { get; }

    private const string CommandName = "/directchat";

    public DirectChat(
        [RequiredVersion("1.0")] ICommandManager commandManager,
        [RequiredVersion("1.0")] IGameConfig gameConfig,
        [RequiredVersion("1.0")] IChatGui chat,
        [RequiredVersion("1.0")] IPluginLog pluginLog)
    {
        CommandManager = commandManager;
        GameConfig = gameConfig;
        Chat = chat;
        PluginLog = pluginLog;

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
