using Barotrauma;
using HarmonyLib;

namespace Multicommands
{
  public partial class Mod
  {
    public void Experiment()
    {
      Logger.Log(Logger.Wrap.IEnumerable("bruh {0} wef{}wef kek {1} lol {2}".Split('{', '}')));
    }
  }
}

