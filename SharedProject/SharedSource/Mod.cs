using Barotrauma;
using Microsoft.Xna.Framework;

namespace Multicommands
{
  public partial class Mod
  {
    public static Mod? Instance { get; private set; }

    public void Init()
    {
      Instance = this;
    }
    public partial void InitProjectSpecific();

    public void OnContentLoaded() { }

    public void Dispose()
    {

      ConsoleInterface.RemoveAllCommands();
      Instance = null;

    }
  }
}

