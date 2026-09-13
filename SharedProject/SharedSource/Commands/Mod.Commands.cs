using Barotrauma;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public static class ModCommands
  {
    public static void Install()
    {
      VanillaConsoleInterface.AddCommand(
        "add", Add_Command, addToStart: false,
        getValidArgs: () => [Mod.CommandManager.Multicommands.Keys.ToArray()],
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
        getValidArgs: () => [Mod.CommandManager.Multicommands.Keys.ToArray()],
        help:
        """
        Syntax: delete multicommand
        deletes multicommand
        """
      );

      VanillaConsoleInterface.AddCommand(
        "remove", Remove_Command, addToStart: false,
        getValidArgs: () => [Mod.CommandManager.Multicommands.Keys.ToArray()],
        help:
        """
        Syntax: remove multicommand [part index]
        Removes last part from multicommand
        If [part index] is specified removes that one
        If there's only 1 part then deletes the command
        """
      );

      VanillaConsoleInterface.AddCommand("print_multicommands", Print_Multicommands_Command,
        getValidArgs: () => [Mod.CommandManager.Multicommands.Keys.ToArray()]
      );
      VanillaConsoleInterface.AddCommand("save_multicommands", Save_Multicommands_Command, help: "you don't need this");
      VanillaConsoleInterface.AddCommand("load_multicommands", Load_Multicommands_Command, help: "you don't need this");
      VanillaConsoleInterface.AddCommand(
        "export_multicommand",
        Export_Multicommand_Command,
        help: "to clipboard",
        getValidArgs: () => [Mod.CommandManager.Multicommands.Keys.ToArray()]
      );
    }

    public static void Export_Multicommand_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log($"which one?");
        return;
      }

      string name = args[0];

      if (Mod.CommandManager.Multicommands.ContainsKey(name))
      {
        Clipboard.SetText(Mod.CommandManager.Multicommands[name].Content);
        Mod.Logger.Log($"command [{Logger.White(name)}] was copied to clipboard");
      }
      else
      {
        Mod.Logger.Log($"no such command");
      }
    }

    public static void Print_Multicommands_Command(string[] args)
    {
      if (args.Length > 0 && Mod.CommandManager.Multicommands.ContainsKey(args[0]))
      {
        Mod.Logger.Log($"{Logger.White(args[0])}   -   {Logger.White(Mod.CommandManager.Multicommands[args[0]].Content)}");
        return;
      }

      // else
      foreach (var (key, command) in Mod.CommandManager.Multicommands)
      {
        Mod.Logger.Log($"{Logger.White(key)}   -   {Logger.White(command.Content)}");
      }
    }

    public static void Save_Multicommands_Command(string[] args)
    {
      Mod.MulticommandsRepo.Save(Mod.CommandManager.Multicommands);
      Mod.Logger.Log("Saved multicommands");
    }

    public static void Load_Multicommands_Command(string[] args)
    {
      Mod.CommandManager.Multicommands.Swap(Mod.MulticommandsRepo.Load());
      Mod.Logger.Log("Loaded multicommands");
    }



    public static void Add_Command(string[] args)
    {
      if (args.Length < 2)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["add"].Help);
        return;
      }

      string name = args[0];

      string content = "";
      if (Mod.CommandManager.Multicommands.ContainsKey(name))
      {
        content = Mod.CommandManager.Multicommands[name].Content;
      }

      string newPart = string.Join(' ', args.Skip(1).Select(
          //HACK vanilla commands strip args from "", so if i just join them i'll miss "" and they won't work
          // I can't change DebugConsole.ExecuteCommand because it'll break vanilla commands
          // I can use my fake commands, but i can't merge their autocomplete with vanilla one
          // I'll have to unwind and rewrite that whole thing to make it work on multiple list of commands
          // And it's not a problem for multicommands because they inherently don't have autocomplete for args
          // But there's a hack, the only use case for "" i see is to wrap args that have spaces
          // So i can just check if there's spaces and add "" 
          // ...
          // profit
          s => s.Contains(' ') ? $"\"{s}\"" : s
        )
      );

      if (content.Trim() == "")
      {
        content = newPart;
      }
      else
      {
        content = string.Join(Mod.Settings.CommandSeparator, content, newPart);
      }

      Mod.CommandManager.Multicommands[name] = new Multicommand(content);

      Mod.Logger.Log($"added part [{Logger.White(newPart)}] to [{Logger.White(name)}]");
      Mod.Logger.Log($"{Logger.White(name)}   -   {Logger.White(content)}");
    }

    public static void Create_Command(string[] args)
    {
      if (args.Length < 2)
      {
        Mod.Logger.Warning(VanillaConsoleInterface.Commands["create"].Help);
        return;
      }

      string name = args[0];


      string content = string.Join(' ', args.Skip(1).Select(
          s => s.Contains(' ') ? $"\"{s}\"" : s //HACK
        )
      );

      Mod.CommandManager.Multicommands[name] = new Multicommand()
      {
        Content = content
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

      string[] parts = multicommand.Content.Split(Mod.Settings.CommandSeparator);

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

      Mod.CommandManager.Multicommands[name] = new Multicommand(
        string.Join(
          Mod.Settings.CommandSeparator,
          parts.Where((part, i) => i != partToDelete).ToArray()
        )
      );

      Mod.Logger.Log($"removed part [{Logger.White(parts[partToDelete])}] from [{Logger.White(name)}]");
      Mod.Logger.Log($"{Logger.White(name)}   -   {Logger.White(Mod.CommandManager.Multicommands[name].Content)}");
    }
  }
}

