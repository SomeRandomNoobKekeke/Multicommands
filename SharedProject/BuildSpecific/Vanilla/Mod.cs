using System.Reflection;
using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public partial class Mod : IBarotraumaPlugin
  {
    public partial void InitBuildSpecific()
    {
      Package = PluginLoader.LoadedPlugins.First(
        plugin => plugin.Assembly == Assembly.GetExecutingAssembly()
      ).Data.ContentPackage;
    }
  }
}

