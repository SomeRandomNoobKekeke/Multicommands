using Barotrauma;

namespace Multicommands
{


  public static class VanillaConsoleInterface
  {
    private static HashSet<DebugConsole.Command> AddedCommands = new();

    public static bool ForceSplit { get; set; } //HACK
    public static void Execute(string command) => DebugConsole.ExecuteCommand(command);
    public static void SplitAndExecute(string command)
    {
      ForceSplit = true;
      DebugConsole.ExecuteCommand(command);
      ForceSplit = false;
    }


    public static bool TrySplitAndExecute(string command)
    {
      if (Mod.Settings.SplitAllCommands || ForceSplit)
      {
        string[] parts = command.Split(Mod.Settings.SplitChar);
        if (parts.Length > 1)
        {
          foreach (string part in parts)
          {
            DebugConsole.ExecuteCommand(part);
          }

          return true;
        }
      }

      return false;
    }

    public static void AddCommand(
      string name,
      Action<string[]> onExecute,
      Func<string[][]> getValidArgs = null,
      bool isCheat = false,
      bool relayToServer = false,
      string help = "",
      bool addToStart = true
    )
    {
      DebugConsole.Command command = new DebugConsole.Command(name, help, onExecute, getValidArgs, isCheat)
      {
#if CLIENT
        RelayToServer = relayToServer
#endif
      };

      AddCommand(command, addToStart);
    }
    public static void AddCommand(DebugConsole.Command command, bool addToStart = true)
    {
      if (AddedCommands.Add(command))
      {
        if (addToStart)
        {
          DebugConsole.Commands.Insert(0, command);
        }
        else
        {
          DebugConsole.Commands.Add(command);
        }
      }
    }

    public static void RemoveCommand(DebugConsole.Command command)
    {
      if (AddedCommands.Remove(command))
      {
        DebugConsole.Commands.Remove(command);
      }
    }

    public static void RemoveAllCommands()
    {
      foreach (var command in AddedCommands)
      {
        DebugConsole.Commands.Remove(command);
      }
      AddedCommands.Clear();
    }
  }
}

