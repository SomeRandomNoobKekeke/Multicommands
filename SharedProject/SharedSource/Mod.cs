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
        Command = "arg1 {0} arg2 {0} arg3 {1}"
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

