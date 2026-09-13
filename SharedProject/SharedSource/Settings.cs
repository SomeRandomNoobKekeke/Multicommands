using System.IO;

namespace Multicommands
{
  public class Settings
  {
    public bool MulticommandsFirst { get; set; } = true;
    public bool SplitVanillaCommands { get; set; } = true;
    public bool Autosave { get; set; } = true;
    public char CommandSeparator { get; set; } = ';';
    public string SavePath { get; set; } = Path.Combine("ModSettings", "MultiCommand", "MultiCommands.xml");
  }
}

