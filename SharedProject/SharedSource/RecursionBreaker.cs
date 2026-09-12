using Barotrauma;

namespace Multicommands
{
  public class RecursionBreaker
  {
    public Action OnBreak { get; set; }

    public bool CanEnter { get; private set; } = true;
    public int Depth { get; private set; }
    public int MaxDepth { get; set; } = 10;

    public bool TryEnter()
    {
      if (!CanEnter) return false;

      Depth++;

      if (Depth >= MaxDepth)
      {
        CanEnter = false;
        OnBreak?.Invoke();
      }

      return CanEnter;
    }
    public void Exit()
    {
      Depth = Math.Max(0, Depth - 1);
      if (Depth == 0) CanEnter = true;
    }
  }
}

