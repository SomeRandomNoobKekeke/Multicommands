using Barotrauma;
using HarmonyLib;
using System.Reflection;

namespace Multicommands
{
  public partial class Mod
  {
    public static Mod? Instance { get; private set; }
    public static bool IsDisposed => Instance == null;

    public static Logger Logger { get; private set; } = new();
    public static CommandManager CommandManager { get; private set; } = new();
    public Harmony Harmony { get; } = new Harmony("multicommands");

    public static Settings Settings { get; private set; } = new();
    public static MulticommandsRepo MulticommandsRepo { get; private set; } = new();


    public void Init()
    {
      Instance = this;
      Experiment();


      CommandManager.Multicommands.Changed += (newValue) =>
      {
        if (Settings.Autosave)
        {
          MulticommandsRepo.Save(newValue);
        }
      };
      CommandManager.Multicommands.Swap(MulticommandsRepo.Load());


      ControlingCommands.Install();
      DebugConsole_Patches.Add(Harmony);
    }
    public partial void InitProjectSpecific();

    public void OnContentLoaded() { }

    public void DestroyStaticVars()
    {
      foreach (FieldInfo fi in typeof(Mod).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
      {
        fi.SetValue(null, null);
      }
    }

    public void Dispose()
    {
      Harmony.UnpatchSelf();
      VanillaConsoleInterface.RemoveAllCommands();

      DestroyStaticVars();
    }
  }
}

