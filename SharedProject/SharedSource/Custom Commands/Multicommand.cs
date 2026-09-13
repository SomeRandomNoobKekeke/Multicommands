using Barotrauma;
using System.Text;
namespace Multicommands
{
  public class Multicommand
  {
    public string Command { get; set; } = "";

    public void Execute(string[] args)
    {
      string[] parts = Command.Split('{', '}');

      // Replace all {0}, {1} with args
      if (parts.Length > 1)
      {
        for (int i = 0; i < parts.Length; i++)
        {
          if (int.TryParse(parts[i], out int index))
          {
            parts[i] = args.ElementAtOrDefault(index);
          }
        }
      }

      VanillaConsoleInterface.SplitAndExecute(String.Join("", parts));
    }
  }
}

