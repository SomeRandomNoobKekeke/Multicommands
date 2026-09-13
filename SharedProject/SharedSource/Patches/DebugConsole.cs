using Barotrauma;
using HarmonyLib;
using static Barotrauma.DebugConsole;
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
        prefix: new HarmonyMethod(typeof(DebugConsole_Patches).GetMethod("DebugConsole_AutoComplete_Replace"))
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
      if (Mod.IsDisposed) return true; // run original

      if (!RecursionBreaker.TryEnter()) return false;

      inputtedCommands = inputtedCommands.Trim();

      if (VanillaConsoleInterface.TrySplitAndExecute(inputtedCommands))
      {
        return false;
      }

      bool handled = Mod.CommandManager.TryExecute(inputtedCommands);

      return !handled;
    }

    // https://github.com/FakeFishGames/Barotrauma/blob/master/Barotrauma/BarotraumaShared/SharedSource/DebugConsole.cs#L2386
    public static bool DebugConsole_AutoComplete_Replace(ref string __result, string command, int increment = 1)
    {
      if (Mod.IsDisposed) return true; // run original

      bool handled = Mod.CommandManager.TryAutoComplete(ref __result, command, increment);

      return !handled;









      string[] splitCommand = ToolBox.SplitCommand(command);
      string[] args = splitCommand.Skip(1).ToArray();

      //if an argument is given or the last character is a space, attempt to autocomplete the argument
      if (args.Length > 0 || (splitCommand.Length > 0 && command.Last() == ' '))
      {
        DebugConsole.Command matchingCommand = commands.Find(c => c.Names.Contains(splitCommand[0].ToIdentifier()));
        if (matchingCommand?.GetValidArgs == null) { __result = command; return false; }

        int autoCompletedArgIndex = args.Length > 0 && command.Last() != ' ' ? args.Length - 1 : args.Length;

        //get all valid arguments for the given command
        string[][] allArgs = matchingCommand.GetValidArgs();
        if (allArgs == null || allArgs.GetLength(0) < autoCompletedArgIndex + 1) { __result = command; return false; }

        if (string.IsNullOrEmpty(currentAutoCompletedCommand))
        {
          currentAutoCompletedCommand = autoCompletedArgIndex > args.Length - 1 ? " " : args.Last();
        }

        //find all valid autocompletions for the given argument
        string[] validArgs = allArgs[autoCompletedArgIndex].Where(arg =>
            currentAutoCompletedCommand.Trim().Length <= arg.Length &&
            arg.Substring(0, currentAutoCompletedCommand.Trim().Length).ToLower() == currentAutoCompletedCommand.Trim().ToLower()).ToArray();

        // add all completions that contain the current argument, to the end of the list
        validArgs = validArgs.Concat(allArgs[autoCompletedArgIndex].Where(arg =>
            arg.ToLower().Contains(currentAutoCompletedCommand.Trim().ToLower()) &&
            !validArgs.Contains(arg))).ToArray();

        if (validArgs.Length == 0) { __result = command; return false; }

        currentAutoCompletedIndex = MathUtils.PositiveModulo(currentAutoCompletedIndex + increment, validArgs.Length);
        string autoCompletedArg = validArgs[currentAutoCompletedIndex];

        //add quotation marks to args that contain spaces
        if (autoCompletedArg.Contains(' ')) autoCompletedArg = '"' + autoCompletedArg + '"';
        for (int i = 0; i < splitCommand.Length; i++)
        {
          if (splitCommand[i].Contains(' ')) splitCommand[i] = '"' + splitCommand[i] + '"';
        }

        __result = string.Join(" ", autoCompletedArgIndex >= args.Length ? splitCommand : splitCommand.Take(splitCommand.Length - 1)) + " " + autoCompletedArg;
        return false;
      }
      else
      {
        if (string.IsNullOrWhiteSpace(currentAutoCompletedCommand))
        {
          currentAutoCompletedCommand = command;
        }

        List<string> matchingCommands = new();


        //============================ added code ===============================
        if (Mod.Settings.MulticommandsFirst)
        {
          foreach (var (name, multicommand) in Mod.CommandManager.Multicommands)
          {
            if (currentAutoCompletedCommand.Length > name.Length) { continue; }
            if (name.StartsWith(currentAutoCompletedCommand))
            {
              matchingCommands.Add(name);
            }
          }
        }
        //=======================================================================

        foreach (DebugConsole.Command c in commands)
        {
          foreach (var name in c.Names)
          {
            if (currentAutoCompletedCommand.Length > name.Value.Length) { continue; }
            if (name.StartsWith(currentAutoCompletedCommand))
            {
              matchingCommands.Add(name.Value);
            }
          }
        }

        //============================ added code ===============================
        if (!Mod.Settings.MulticommandsFirst)
        {
          foreach (var (name, c) in Mod.CommandManager.Multicommands)
          {
            if (currentAutoCompletedCommand.Length > name.Length) { continue; }
            if (name.StartsWith(currentAutoCompletedCommand))
            {
              matchingCommands.Add(name);
            }
          }
        }

        foreach (var (name, c) in Mod.CommandManager.OtherCommands)
        {
          if (currentAutoCompletedCommand.Length > name.Length) { continue; }
          if (name.StartsWith(currentAutoCompletedCommand))
          {
            matchingCommands.Add(name);
          }
        }
        //=======================================================================

        if (matchingCommands.Count == 0)
        {
          __result = command;
          return false;
        }

        currentAutoCompletedIndex = MathUtils.PositiveModulo(currentAutoCompletedIndex + increment, matchingCommands.Count);
        __result = matchingCommands[currentAutoCompletedIndex];
        return false;
      }
    }
  }
}

