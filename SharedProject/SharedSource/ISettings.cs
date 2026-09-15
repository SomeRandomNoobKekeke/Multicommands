using System.IO;
using System.Xml.Linq;
using Barotrauma.Plugins;

namespace Multicommands
{
  public interface ISettings
  {
    public bool MulticommandsFirst { get; set; }
    public bool SplitVanillaCommands { get; set; }
    public bool Autosave { get; set; }
    public char CommandSeparator { get; set; }
    public string SavePath { get; set; }

    public void Print();
  }
}

