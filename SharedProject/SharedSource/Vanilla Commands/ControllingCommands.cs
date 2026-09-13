using Barotrauma;

namespace Multicommands
{
  public static class ControlingCommands
  {
    public static void Install()
    {
      VanillaConsoleInterface.AddCommand(
        "add", Add_Command, addToStart: false,
        help:
        """
        Syntax: add multicommand part
        Adds new part to the end of multicommand
        Creates it if it doesn't exist
        """
      );

      VanillaConsoleInterface.AddCommand(
        "create", Create_Command, addToStart: false,
        help:
        """
        Syntax: create multicommand content
        Creates new multicommand with content
        Rewrites it if it already exist
        Doesn't get splitted
        """
      );

      VanillaConsoleInterface.AddCommand(
        "delete", Delete_Command, addToStart: false,
        help:
        """
        Syntax: delete multicommand
        deletes multicommand
        """
      );

      VanillaConsoleInterface.AddCommand(
        "remove", Remove_Command, addToStart: false,
        help:
        """
        Syntax: remove multicommand [part index]
        Removes last part from multicommand
        If [part index] is specified removes that one
        If there's only 1 part then deletes the command
        """
      );

      VanillaConsoleInterface.AddCommand("printmulticommands", PrintMulticommands_Command);
    }


    public static void PrintMulticommands_Command(string[] args)
    {
      foreach (var (key, command) in Mod.CommandManager.Multicommands)
      {
        Mod.Logger.Log($"{key} - {command.Command}");
      }
    }

    public static void Add_Command(string[] args)
    {
      if (args.Length < 2)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["add"].Help);
        return;
      }

      string name = args[0];

      if (!Mod.CommandManager.Multicommands.ContainsKey(name))
      {
        Mod.CommandManager.Multicommands[name] = new Multicommand();
      }

      Multicommand multicommand = Mod.CommandManager.Multicommands[name];
      string newPart = string.Join(' ', args.Skip(1));

      if (multicommand.Command.Trim() == "")
      {
        multicommand.Command = newPart;
      }
      else
      {
        multicommand.Command = string.Join(Mod.Settings.SplitChar, multicommand.Command, newPart);
      }

      Mod.Logger.Log($"added part [{Logger.White(newPart)}] to [{Logger.White(name)}]");
      Mod.Logger.Log($"{Logger.White(name)}   -   {Logger.White(multicommand.Command)}");
    }

    public static void Create_Command(string[] args)
    {
      if (args.Length < 2)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["create"].Help);
        return;
      }

      string name = args[0];

      string content = string.Join(' ', args.Skip(1));

      Mod.CommandManager.Multicommands[name] = new Multicommand()
      {
        Command = content
      };

      Mod.Logger.Log($"{Logger.White(name)}   -   {Logger.White(content)}");
    }

    public static void Delete_Command(string[] args)
    {
      if (args.Length < 1)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["delete"].Help);
        return;
      }

      string name = args[0];

      if (Mod.CommandManager.Multicommands.ContainsKey(name))
      {
        Mod.CommandManager.Multicommands.Remove(name);
        Mod.Logger.Log($"deleted [{Logger.White(name)}]");
      }
      else
      {
        Mod.Logger.Log($"no such multicommand");
      }
    }

    public static void Remove_Command(string[] args)
    {
      if (args.Length < 1)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["remove"].Help);
        return;
      }

      string name = args[0];

      if (!Mod.CommandManager.Multicommands.ContainsKey(name))
      {
        Mod.Logger.Log($"no such multicommand");
        return;
      }

      Multicommand multicommand = Mod.CommandManager.Multicommands[name];

      string[] parts = multicommand.Command.Split(Mod.Settings.SplitChar);

      if (parts.Length == 1)
      {
        Mod.CommandManager.Multicommands.Remove(name);
        Mod.Logger.Log($"deleted [{Logger.White(name)}]");
        return;
      }

      int partToDelete = parts.Length - 1;
      if (args.Length > 1)
      {
        if (int.TryParse(args[1], out int i))
        {
          if (i < 0 || i > parts.Length - 1)
          {
            Mod.Logger.Warning($"part index out of bounds, should be in [0,{parts.Length - 1}]");
            return;
          }

          partToDelete = i;
        }
        else
        {
          Mod.Logger.Warning("2 arg should be int");
          return;
        }
      }

      multicommand.Command = string.Join(
        Mod.Settings.SplitChar,
        parts.Where((part, i) => i != partToDelete).ToArray()
      );

      Mod.Logger.Log($"removed part [{Logger.White(parts[partToDelete])}] from [{Logger.White(name)}]");
      Mod.Logger.Log($"{Logger.White(name)}   -   {Logger.White(multicommand.Command)}");
    }
  }
}

