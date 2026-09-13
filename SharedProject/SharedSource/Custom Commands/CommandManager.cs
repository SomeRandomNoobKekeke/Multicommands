
namespace Multicommands
{
  public class CommandManager
  {
    public Dictionary<string, Multicommand> Commands { get; } = new();

    public bool TryExecute(string command)
    {
      if (string.IsNullOrEmpty(command)) return false;

      string[] parts = command.Split(' ');
      string name = parts[0];
      string[] args = parts.Skip(1).Where(arg => arg != "").ToArray();

      if (!Commands.ContainsKey(name)) return false;

      try
      {
        Commands[name].Execute(args);
      }
      catch (Exception e)
      {
        Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
      }

      return true;
    }

    private bool TryRestoreCommandName(ref string incompleteName)
    {
      foreach (string name in Commands.Keys)
      {
        if (name.Contains(incompleteName))
        {
          incompleteName = name;
          return true;
        }
      }
      return false;
    }

    public bool TryAutoComplete(ref string __result, string command, int increment)
    {
      if (string.IsNullOrEmpty(command)) return false;

      string[] parts = command.Split(' ');
      string name = parts[0];
      string[] args = parts.Skip(1).Where(arg => arg != "").ToArray();

      if (!Commands.ContainsKey(name))
      {
        if (TryRestoreCommandName(ref name))
        {
          __result = $"{name} ";
          return true;
        }

        return false;
      }

      int depthChange = increment;
      if (depthChange == 0 && (command.Last() == ' ' || args.Length == 0))
      {
        depthChange = 1;
      }

      try
      {
        string[] autocompletedArgs = Commands[name].Autocomplete(args, depthChange);
        __result = $"{name} {string.Join(' ', autocompletedArgs)}";
      }
      catch (Exception e)
      {
        Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
        __result = command;
      }

      return true;
    }
  }
}

