using Barotrauma;
using System.Text;
namespace Multicommands
{
  public class Multicommand : IConsoleCommand
  {
    public string Command { get; set; } = "";

    public string[][] Hints
    {
      get => Crawler.Hints;
      set => Crawler.Hints = value;
    }

    private HintsCrawler Crawler = new();

    public string[] Autocomplete(string[] args, int depthChange)
    {
      if (Hints is null) return args;

      if (depthChange == 1) return Crawler.MoveDeeper(args);

      if (Crawler.TryFixLastArg(args)) return args;

      return Crawler.Cycle(args, 1);
    }


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

