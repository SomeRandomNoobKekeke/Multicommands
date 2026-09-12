using System;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;

namespace Multicommands
{
  public class Multicommand : DebugConsole.Command
  {
    public Multicommand(
      string name,
      string help,
      Action<string[]> onExecute,
      Func<string[][]> getValidArgs = null,
      bool isCheat = false
    ) : base(name, help, onExecute, getValidArgs, isCheat)
    {

    }
  }
}

