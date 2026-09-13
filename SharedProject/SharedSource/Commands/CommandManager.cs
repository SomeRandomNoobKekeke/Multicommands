
namespace Multicommands
{
  public class CommandManager
  {
    public Dictionary<string, ConsoleCommand> OtherCommands { get; } = new();
    public ReactiveDict<string, Multicommand> Multicommands { get; } = new();

    public bool TryExecute(string command)
    {
      if (string.IsNullOrEmpty(command)) return false;

      string[] parts = command.Split(' ');
      string name = parts[0];
      string[] args = parts.Skip(1).Where(arg => arg != "").ToArray();


      if (OtherCommands.ContainsKey(name))
      {
        try
        {
          VanillaConsoleInterface.NewCommand(command);
          OtherCommands[name].Execute(args);
        }
        catch (Exception e)
        {
          Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
        }
        return true;
      }


      if (Multicommands.ContainsKey(name))
      {
        try
        {
          VanillaConsoleInterface.NewCommand(command);
          Multicommands[name].Execute(args);
        }
        catch (Exception e)
        {
          Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
        }
        return true;
      }

      return false;
    }


    private bool TryRestoreCommandName(ref string incompleteName)
    {
      foreach (string name in Multicommands.Keys)
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

      int depthChange = increment;
      if (depthChange == 0 && command.Last() == ' ')
      {
        depthChange = 1;
      }

      if (!Multicommands.ContainsKey(name))
      {
        if (TryRestoreCommandName(ref name))
        {
          __result = name;
          return true;
        }

        return false;
      }

      Mod.Logger.LogVars(depthChange, args.Length);

      if (depthChange == 0 && args.Length == 0)
      {
        return false; // cycle to other commands
      }

      try
      {
        string[] autocompletedArgs = Multicommands[name].Autocomplete(args, depthChange);
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