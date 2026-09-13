
namespace Multicommands
{
  public class CommandManager
  {
    public ReactiveDict<string, Multicommand> Multicommands { get; } = new();

    public bool TryExecute(string command)
    {
      if (string.IsNullOrEmpty(command)) return false;

      string[] parts = command.Split(' ');
      string name = parts[0];
      string[] args = parts.Skip(1).Where(arg => arg != "").ToArray();

      if (!Multicommands.ContainsKey(name)) return false;

      try
      {
        Multicommands[name].Execute(args);
      }
      catch (Exception e)
      {
        Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
      }

      return true;
    }
  }
}