using Barotrauma;
using HarmonyLib;
using System.Reflection;
using System.IO;
using Barotrauma.Plugins;

namespace Multicommands
{
  public partial class Mod
  {
    public void DefineSettings()
    {
      SettingsService.RegisterSetting(new BooleanSetting(
        $"MulticommandsFirst".ToIdentifier(),
        true, // default
        label: $"MultiCommands First"
      )
      {
        ShowInUI = true,
        ToolTip = "in tab index",
      });

      SettingsService.RegisterSetting(new BooleanSetting(
        $"SplitVanillaCommands".ToIdentifier(),
        true, // default
        label: $"Split Vanilla Commands"
      )
      {
        ShowInUI = true,
      });

      SettingsService.RegisterSetting(new BooleanSetting(
        $"Autosave".ToIdentifier(),
        true, // default
        label: $"Autosave MultiCommands"
      )
      {
        ShowInUI = true,
      });

      SettingsService.RegisterSetting(new StringSetting(
        $"CommandSeparator".ToIdentifier(),
        ";",
        label: $"Command Separator"
      )
      {
        ShowInUI = true,
        MaxSize = 1,
      });

      SettingsService.RegisterSetting(new StringSetting(
        $"SavePath".ToIdentifier(),
        Path.Combine("ModSettings", "MultiCommand", "MultiCommands.xml"),
        label: $"Save Path"
      )
      {
        ShowInUI = true,
        ToolTip = "Where MultiCommands.xml is saved",
      });
    }
  }
}

