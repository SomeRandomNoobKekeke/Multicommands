using Barotrauma;

namespace Multicommands
{
  public interface IConsoleCommand
  {
    public void Execute(string[] args);
    public string[] Autocomplete(string[] args, int depthChange);
  }
}

