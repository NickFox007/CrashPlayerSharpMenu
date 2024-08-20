using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CrashPlayerSharpApi;
using MenuManager;

namespace CrashPlayerSharpMenu;
public class CrashPlayerSharpMenu : BasePlugin
{
    public override string ModuleName => "CrashPlayer CSharp AdminMenu [Module]";
    public override string ModuleVersion => "0.4";

    private IMenuApi? _menu_api;
    private ICrashPlayerApi? _cpc_api;
    private readonly PluginCapability<IMenuApi> _pluginMenu = new("menu:nfcore");
    private readonly PluginCapability<ICrashPlayerApi> _pluginCPC = new("crashplayer:nfcore");


    public override void OnAllPluginsLoaded(bool hotReload)
    {
        _menu_api = _pluginMenu.Get();
        _cpc_api = _pluginCPC.Get();
    }


    bool callback(CCSPlayerController player, CCSPlayerController target)
    {
        
        if (!player.Equals(null))
        {
            _cpc_api.CPC_CrashPlayer(target);
            player.PrintToChat("Крашим игрока " + target.PlayerName);
        }
        return true;
    }

    [RequiresPermissions("@css/root")]
    [ConsoleCommand("css_crash", "Crash choosen player!")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnCommand(CCSPlayerController? player, CommandInfo command)
    {        
        var menu = _menu_api.NewMenu("CrashMenu");
        foreach (var target in Utilities.GetPlayers())
            if (CheckPlayer(target))
                menu.AddMenuOption(target.PlayerName, (player, option) => { callback(player, target);});
        menu.Open(player);        
    }

    private bool CheckPlayer(CCSPlayerController player)
    {
        if (player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected && !player.IsBot) return true;
        else return false;
    }


}