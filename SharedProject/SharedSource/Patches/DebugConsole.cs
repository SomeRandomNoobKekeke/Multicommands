using Barotrauma;
using HarmonyLib;

namespace Multicommands
{
  public static class DebugConsole_Patches
  {
    public static void Add(Harmony Harmony)
    {
      Harmony.Patch(
        original: typeof(DebugConsole).GetMethod("ExecuteCommand", AccessTools.all),
        prefix: new HarmonyMethod(typeof(DebugConsole_Patches).GetMethod("DebugConsole_ExecuteCommand_Prefix"))
      );

      Harmony.Patch(
        original: typeof(DebugConsole).GetMethod("AutoComplete", AccessTools.all),
        prefix: new HarmonyMethod(typeof(DebugConsole_Patches).GetMethod("DebugConsole_AutoComplete_Prefix"))
      );
    }

    public static bool DebugConsole_ExecuteCommand_Prefix(string inputtedCommands)
    {
      inputtedCommands = inputtedCommands.Trim();

      string[] parts = inputtedCommands.Split(';');

      if (parts.Length > 1)
      {
        foreach (string part in parts)
        {
          DebugConsole.ExecuteCommand(part);
        }

        return false;
      }


      bool handled = Mod.CommandManager.TryExecute(inputtedCommands);
      return !handled;
    }

    public static bool DebugConsole_AutoComplete_Prefix(ref string __result, string command, int increment = 1)
    {
      bool handled = Mod.CommandManager.TryAutoComplete(ref __result, command, increment);

      return !handled;
    }
  }
}

