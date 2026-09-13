using Barotrauma;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public static class OtherCommands
  {
    public static void Install()
    {
      VanillaConsoleInterface.AddCommand("speak", Speak_Command, help: "half-assed, don't use");
    }

    public static void Speak_Command(string[] args)
    {
      if (args.Length == 0) return;

      string msg = string.Join(' ', args.Select(
          s => s.Contains(' ') ? $"\"{s}\"" : s
        )
      );

      if (msg == "") return;

      Character.Controlled?.Speak(msg);
      DebugConsole.ExecuteCommand($"say \"{msg}\"");
    }

  }
}

