using Barotrauma;

namespace Multicommands
{
  public static class VanillaConsoleInterface
  {
    public static Dictionary<string, DebugConsole.Command> Commands { get; set; } = new();

    public static void Execute(string command) => DebugConsole.ExecuteCommand(command);
    public static void SplitAndExecute(string command)
    {
      string[] parts = command.Split(Mod.Settings.CommandSeparator);

      foreach (string part in parts)
      {
        DebugConsole.ExecuteCommand(part);
      }
    }

    public static void NewCommand(string command) => DebugConsole.NewCommand(command);

    public static bool IsSpecialCommand(string command)
    {
      if (command.StartsWith("create")) return true;
      return false;
    }

    /// <returns> true if command was composite </returns>
    public static bool TrySplitAndExecute(string command)
    {
      if (Mod.Settings.SplitVanillaCommands && !IsSpecialCommand(command))
      {
        string[] parts = command.Split(Mod.Settings.CommandSeparator);
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
      if (DebugConsole.Commands.Any(c => c.Names[0].Value == command.Names[0].Value))
      {
        Mod.Logger.Warning($"Can't add [{Logger.White(command.Names[0].Value)}] command, it already exists");
        return;
      }

      Commands[command.Names[0].Value] = command;

      if (addToStart)
      {
        DebugConsole.Commands.Insert(0, command);
      }
      else
      {
        DebugConsole.Commands.Add(command);
      }
    }

    public static void RemoveCommand(DebugConsole.Command command)
    {
      if (!Commands.ContainsKey(command.Names[0].Value)) return;

      Commands.Remove(command.Names[0].Value);
      DebugConsole.Commands.Remove(command);
    }

    public static void RemoveAllCommands()
    {
      foreach (var command in Commands.Values)
      {
        DebugConsole.Commands.Remove(command);
      }
      Commands.Clear();
    }
  }
}

