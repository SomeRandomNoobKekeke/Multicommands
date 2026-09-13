using Barotrauma;
using System.Text;
namespace Multicommands
{
  public record Multicommand(string Content = "")
  {
    public static Multicommand FromParts(IEnumerable<string> parts)
    {
      return new Multicommand(string.Join(Mod.Settings.CommandSeparator, parts));
    }

    public void Execute(string[] args)
    {
      string[] parts = Content.Split('{', '}');

      // Replace all {0}, {1} with args
      //FIXME it can instead replace things outside } { if they look parseable to int, have no idea why would you do that though
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

