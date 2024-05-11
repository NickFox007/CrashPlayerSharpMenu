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
    public override string ModuleVersion => "0.2";

    private IMenuApi? _menu_api;
    private ICrashPlayerApi? _cpc_api;
    private readonly PluginCapability<IMenuApi> _pluginMenu = new("menu:nfcore");
    private readonly PluginCapability<ICrashPlayerApi> _pluginCPC = new("crashplayer:nfcore");


    public override void OnAllPluginsLoaded(bool hotReload)
    {
        _menu_api = _pluginMenu.Get();
        _cpc_api = _pluginCPC.Get();
        //if (_cpc_api == null || _menu_api == null) return;
    }


    bool callback(CCSPlayerController player)
    {
        
        if (!player.Equals(null))
        {
            _cpc_api.CPC_CrashPlayer(player);
            player.PrintToChat("Крашим игрока " + player.PlayerName);
        }
        return true;
    }

    /* TEST COMMAND */
    [RequiresPermissions("@css/root")]
    [ConsoleCommand("css_crash", "Crash choosen player!")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnCommand(CCSPlayerController? player, CommandInfo command)
    {
        
        var menu = _menu_api.NewMenu("CrashMenu");
        foreach (var target in Utilities.GetPlayers())
            if (CheckPlayer(target))
                menu.AddMenuOption(target.PlayerName, (player, option) => { callback(target);});

        menu.Open(player);
        
    }

    private bool CheckPlayer(CCSPlayerController player)
    {
        if (player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected && !player.IsBot) return true;
        else return false;
    }
    /* TEST COMMAND */


}