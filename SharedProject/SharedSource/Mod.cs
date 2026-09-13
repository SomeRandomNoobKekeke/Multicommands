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

    public static Settings Settings { get; private set; } = new();


    public void Init()
    {
      Instance = this;
      Experiment();

      CommandManager.Multicommands["cqwe"] = new Multicommand()
      {
        Command = "123",
      };

      ControlingCommands.Install();
      DebugConsole_Patches.Add(Harmony);
    }
    public partial void InitProjectSpecific();

    public void OnContentLoaded() { }

    public void Dispose()
    {
      Harmony.UnpatchSelf();
      VanillaConsoleInterface.RemoveAllCommands();
      CommandManager = null;
      Instance = null;

    }
  }
}

