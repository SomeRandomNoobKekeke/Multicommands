using Barotrauma;
using Microsoft.Xna.Framework;
using Barotrauma.LuaCs;

namespace Multicommands
{
  public partial class Mod : IAssemblyPlugin
  {
    public IPluginManagementService PluginService { get; set; }

    public partial void InitBuildSpecific()
    {
      if (!PluginService.TryGetPackageForPlugin<Mod>(out Package))
      {
        throw new ExecutionEngineException("how are you running this code then?");
      }
    }
    public void Initialize() => Init();
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
  }

}

