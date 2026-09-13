using Barotrauma;

namespace Multicommands
{
  /// <summary>
  /// Works but not used
  /// </summary>
  public class ConsoleCommand : IConsoleCommand
  {
    public Action<string[]> Action { get; set; }
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
      Action?.Invoke(args);
    }
  }
}

