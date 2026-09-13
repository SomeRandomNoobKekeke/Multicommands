using System.IO;
using System.Xml.Linq;

namespace Multicommands
{
  public class MulticommandsRepo
  {
    public string FilePath => Mod.Settings.SavePath;
    public string Dir => Path.GetDirectoryName(FilePath);

    public ReactiveDict<string, Multicommand> DefaultCommands => new()
    {
      ["setskills"] = Multicommand.FromParts([
        "setskill weapons {0}",
        "setskill medical {0}",
        "setskill electrical {0}",
        "setskill mechanical {0}",
        "setskill helm {0}",
      ])
    };

    public ReactiveDict<string, Multicommand> Load()
    {
      if (!File.Exists(FilePath)) return DefaultCommands;

      XDocument xdoc = XDocument.Load(FilePath);

      ReactiveDict<string, Multicommand> commands = [];

      foreach (XElement commandBlock in xdoc.Root.Elements())
      {
        commands[commandBlock.Name.ToString()] = new Multicommand()
        {
          Content = string.Join(
            Mod.Settings.CommandSeparator,
            commandBlock.Elements().Select(e => e.Value)
          )
        };
      }

      return commands;
    }

    public void Save(ReactiveDict<string, Multicommand> commands)
    {
      if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);

      XDocument xdoc = new XDocument();
      xdoc.Add(new XElement("MultiCommands"));

      foreach (var (name, multicommand) in commands)
      {
        XElement commandBlock = new XElement(name);
        string[] parts = multicommand.Content.Split(Mod.Settings.CommandSeparator);
        foreach (string part in parts)
        {
          commandBlock.Add(new XElement("Command", part));
        }
        xdoc.Root!.Add(commandBlock);
      }

      xdoc.Save(FilePath);
    }
  }
}

