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

      Harmony.Patch(
        original: typeof(DebugConsole).GetMethod("Update", AccessTools.all),
        postfix: new HarmonyMethod(typeof(DebugConsole_Patches).GetMethod("DebugConsole_Update_Postfix"))
      );
    }

    private static RecursionBreaker RecursionBreaker = new()
    {
      OnBreak = () => Mod.Logger.Warning("<<========  Max recursion depth reached  ========>>"),
      MaxDepth = 100,
    };

    public static void DebugConsole_Update_Postfix() => RecursionBreaker.Reset();
    public static bool DebugConsole_ExecuteCommand_Prefix(string inputtedCommands)
    {
      if (!RecursionBreaker.TryEnter()) return false;

      inputtedCommands = inputtedCommands.Trim();


      if (VanillaConsoleInterface.TrySplitAndExecute(inputtedCommands))
      {
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

