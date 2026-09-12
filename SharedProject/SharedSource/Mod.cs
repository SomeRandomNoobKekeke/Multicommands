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
      Experiment();

      CommandManager.Commands.Add("kek", new Multicommand()
      {
        Command = "lol;kek;kek"
      });

      CommandManager.Commands.Add("lol", new Multicommand()
      {
        Command = "qwe;qwe;qwe"
      });

      CommandManager.Commands.Add("qwe", new Multicommand()
      {
        Command = "1;2;3"
      });

      DebugConsole_Patches.Add(Harmony);
    }
    public partial void InitProjectSpecific();

    public void OnContentLoaded() { }

    public void Dispose()
    {
      Harmony.UnpatchSelf();
      ConsoleInterface.RemoveAllCommands();
      CommandManager = null;
      Instance = null;

    }
  }
}

