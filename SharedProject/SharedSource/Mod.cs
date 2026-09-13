using Barotrauma;
using HarmonyLib;
using System.Reflection;
using System.IO;

namespace Multicommands
{
  public partial class Mod
  {
    public static Mod? Instance { get; private set; }
    public static bool IsDisposed => Instance == null;

    public static Logger Logger { get; private set; } = new();
    public static CommandManager CommandManager { get; private set; } = new();


    public static Settings Settings { get; private set; } = new();
    public static MulticommandsRepo MulticommandsRepo { get; private set; } = new();

    public Harmony Harmony { get; } = new Harmony("multicommands");

    public ContentPackage Package;
    public partial void InitBuildSpecific();

    public void Init()
    {
      Instance = this;
      InitBuildSpecific();

      Mod.Logger.Log(Package.Dir);

      Experiment();

      Settings.Load(Path.Combine(Package.Dir, "Settings.xml"));
      // Settings.Print();

      CommandManager.Multicommands.Swap(MulticommandsRepo.Load());
      CommandManager.Multicommands.Changed += (newValue) =>
      {
        if (Settings.Autosave)
        {
          MulticommandsRepo.Save(newValue);
        }
      };

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

