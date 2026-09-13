using System.IO;
using System.Xml.Linq;

namespace Multicommands
{
  public class Settings
  {
    public bool MulticommandsFirst { get; set; } = true;
    public bool SplitVanillaCommands { get; set; } = true;
    public bool Autosave { get; set; } = true;
    public char CommandSeparator { get; set; } = ';';
    public string SavePath { get; set; } = Path.Combine("ModSettings", "MultiCommand", "MultiCommands.xml");

    public void Print()
    {
      Mod.Logger.LogVars(MulticommandsFirst);
      Mod.Logger.LogVars(SplitVanillaCommands);
      Mod.Logger.LogVars(Autosave);
      Mod.Logger.LogVars(CommandSeparator);
      Mod.Logger.LogVars(SavePath);
    }

    /// <summary>
    /// Half-assed untill barodevs add mod settings to vanilla
    /// </summary>
    public void Load(string path)
    {
      XDocument xdoc = XDocument.Load(path);

      if (bool.TryParse(xdoc.Root.Element("MulticommandsFirst")?.Value, out bool b))
      {
        MulticommandsFirst = b;
      }

      if (bool.TryParse(xdoc.Root.Element("SplitVanillaCommands")?.Value, out b))
      {
        SplitVanillaCommands = b;
      }

      if (bool.TryParse(xdoc.Root.Element("Autosave")?.Value, out b))
      {
        Autosave = b;
      }

      if (!string.IsNullOrEmpty(xdoc.Root.Element("CommandSeparator")?.Value))
      {
        CommandSeparator = xdoc.Root.Element("CommandSeparator").Value[0];
      }

      if (xdoc.Root.Element("SavePath")?.Value != null)
      {
        try
        {
          SavePath = Path.GetFullPath(xdoc.Root.Element("SavePath").Value);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"invalid path in Multicomand Settings");
          Mod.Logger.Error(xdoc.Root.Element("SavePath").Value);
          Mod.Logger.Error(Logger.Wrap.ExceptionMessage(e));
        }
      }
    }
  }
}

