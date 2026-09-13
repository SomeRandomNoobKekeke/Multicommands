using Barotrauma;

namespace Multicommands
{
  public class HintsCrawler
  {
    public string[][] Hints { get; set; }


    private bool FixArg(ref string arg, string[] options)
    {
      if (options.Length == 0) return false;
      if (options.Contains(arg)) return false;

      for (int i = 0; i < options.Length; i++)
      {
        if (options[i].Contains(arg))
        {
          arg = options[i];
          return true;
        }
      }

      arg = options[0];
      return true;
    }


    public bool TryFixLastArg(string[] args)
    {
      if (args.Length > Hints.Length) return false;
      return FixArg(ref args[args.Length - 1], Hints[args.Length - 1]);
    }

    public bool TryFixArgs(string[] args)
    {
      bool somethingFixed = false;

      for (int i = 0; i < Math.Min(args.Length, Hints.Length); i++)
      {
        somethingFixed |= FixArg(ref args[i], Hints[i]);
      }

      return somethingFixed;
    }

    public bool CanMoveDeeper(string[] args) => args.Length + 1 <= Hints.Length;
    public string[] MoveDeeper(string[] args)
    {
      if (CanMoveDeeper(args))
      {
        return args.Append(Hints[args.Length][0]).ToArray();
      }
      else
      {
        return args;
      }
    }

    public string[] Cycle(string[] args, int increment)
    {
      if (Hints.Length < args.Length) return args;

      string[] hints = Hints[args.Length - 1];
      if (hints.Length == 0) return args;

      int i = hints.IndexOf(args.Last());
      i = (i + increment) % hints.Length;

      args[args.Length - 1] = hints[i];
      return args;
    }

  }
}

