using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;
using Barotrauma.LuaCs;

namespace Multicommands
{
  public partial class Mod : IAssemblyPlugin
  {
    public void Initialize() => Init();
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
  }

}

