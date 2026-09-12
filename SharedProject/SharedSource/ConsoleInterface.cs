using System;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public static class ConsoleInterface
  {
    private static HashSet<DebugConsole.Command> AddedCommands = new();

    public static void AddCommand(DebugConsole.Command command, bool addToStart = false)
    {
      if (AddedCommands.Add(command))
      {
        if (addToStart)
        {
          DebugConsole.Commands.Insert(0, command);
        }
        else
        {
          DebugConsole.Commands.Add(command);
        }
      }
    }

    public static void AddCommands(IEnumerable<DebugConsole.Command> commands, bool addToStart = false)
    {
      foreach (var command in commands)
      {
        AddCommand(command, addToStart);
      }
    }

    public static void RemoveCommand(DebugConsole.Command command)
    {
      if (AddedCommands.Remove(command))
      {
        DebugConsole.Commands.Remove(command);
      }
    }

    public static void RemoveAllCommands()
    {
      foreach (var command in AddedCommands)
      {
        DebugConsole.Commands.Remove(command);
      }
      AddedCommands.Clear();
    }
  }
}

