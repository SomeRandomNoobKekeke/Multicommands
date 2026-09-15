using System.IO;
using System.Xml.Linq;
using Barotrauma.Plugins;
using Barotrauma;

namespace Multicommands
{
  public class Settings : ISettings
  {
    private BooleanSetting _MulticommandsFirst; public bool MulticommandsFirst
    {
      get => _MulticommandsFirst.Value;
      set => _MulticommandsFirst.Set(value);
    }

    private BooleanSetting _SplitVanillaCommands; public bool SplitVanillaCommands
    {
      get => _SplitVanillaCommands.Value;
      set => _SplitVanillaCommands.Set(value);
    }

    private BooleanSetting _Autosave; public bool Autosave
    {
      get => _Autosave.Value;
      set => _Autosave.Set(value);
    }

    private StringSetting _CommandSeparator; public char CommandSeparator
    {
      get
      {
        if (!string.IsNullOrEmpty(_CommandSeparator.Value))
        {
          return _CommandSeparator.Value[0];
        }
        return _CommandSeparator.DefaultValue[0]!;
      }
      set
      {
        _CommandSeparator.Set(value.ToString());
      }
    }

    private StringSetting _SavePath; public string SavePath
    {
      get => _SavePath.Value;
      set => _SavePath.Set(value);
    }

    public void Print()
    {
      Mod.Logger.Log("Settings:");
      Mod.Logger.LogVars(MulticommandsFirst);
      Mod.Logger.LogVars(SplitVanillaCommands);
      Mod.Logger.LogVars(Autosave);
      Mod.Logger.LogVars(CommandSeparator);
      Mod.Logger.LogVars(SavePath);
    }

    public Settings(ISettingsService SettingsService)
    {
      _MulticommandsFirst = SettingsService.RetrieveSetting<BooleanSetting>("MulticommandsFirst".ToIdentifier());
      _SplitVanillaCommands = SettingsService.RetrieveSetting<BooleanSetting>("SplitVanillaCommands".ToIdentifier());
      _Autosave = SettingsService.RetrieveSetting<BooleanSetting>("Autosave".ToIdentifier());
      _CommandSeparator = SettingsService.RetrieveSetting<StringSetting>("CommandSeparator".ToIdentifier());
      _SavePath = SettingsService.RetrieveSetting<StringSetting>("SavePath".ToIdentifier());
    }
  }
}

