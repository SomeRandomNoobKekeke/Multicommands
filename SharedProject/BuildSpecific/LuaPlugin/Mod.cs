using Barotrauma;
using Microsoft.Xna.Framework;
using Barotrauma.LuaCs;

namespace Multicommands
{
  public partial class Mod : IAssemblyPlugin
  {
    public static ISettings Settings { get; private set; } = new Settings();
    public IPluginManagementService PluginService { get; set; }
    public ContentPackage Package;

    public partial void InitBuildSpecific()
    {
      if (!PluginService.TryGetPackageForPlugin<Mod>(out Package))
      {
        throw new ExecutionEngineException("how are you running this code then?");
      }

      Settings.Load(Path.Combine(Package.Dir, "Settings.xml"));
    }
    public void Initialize() => Init();
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
  }

}

