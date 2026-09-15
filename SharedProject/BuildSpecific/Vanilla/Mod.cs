using System.Reflection;
using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public partial class Mod : IBarotraumaPlugin
  {
    public static ISettingsService SettingsService = PluginServiceProvider.GetService<ISettingsService>();
    public static ISettings Settings { get; private set; }
    public ContentPackage Package;

    public partial void InitBuildSpecific()
    {
      Package = PluginLoader.LoadedPlugins.First(
        plugin => plugin.Assembly == Assembly.GetExecutingAssembly()
      ).Data.ContentPackage;

      DefineSettings();
      Settings = new Settings(SettingsService);

      SettingsService.Load();
      // Settings.Print();
    }

    public partial void DisposeBuildSpecific()
    {
      SettingsService.Save();
    }
  }
}

