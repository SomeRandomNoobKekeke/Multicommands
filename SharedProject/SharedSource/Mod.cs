using Barotrauma;
using HarmonyLib;

namespace Multicommands
{
  public partial class Mod
  {
    public static Mod? Instance { get; private set; }
    public static Logger Logger { get; private set; } = new();
    public static CommandManager CommandManager { get; private set; } = new();
    public Harmony Harmony { get; } = new Harmony("multicommands");


    public void Init()
    {
      Instance = this;


      CommandManager.Commands.Add("kek", new ConsoleCommand()
      {
        Action = (args) => Logger.Log($"hi {Logger.Wrap.IEnumerable(args)}"),
        Hints = [["1", "2", "3", "4"], ["q", "w", "e", "r"]],
      });

      DebugConsole_Patches.Add(Harmony);
    }
    public partial void InitProjectSpecific();

    public void OnContentLoaded() { }

    public void Dispose()
    {
      Harmony.UnpatchSelf();
      CommandManager = null;
      Instance = null;

    }
  }
}

