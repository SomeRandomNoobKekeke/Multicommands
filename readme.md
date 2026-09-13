Source code for [Multicommands [DLC testing]](https://steamcommunity.com/sharedfiles/filedetails/?id=3800877071) Barotrauma C# mod  

It's chimera mod: it's based on [BaseBaroPlugin](https://github.com/FakeFishGames/BaseBaroPlugin) and can be compiled into native vanilla c# plugin, but at the same time it has `filelist.xml` and `ModConfig.xml` so it can be placed in LocalMods and be loaded by Luatrauma or [LuaCsForBarotraumaPlugin](https://steamcommunity.com/sharedfiles/filedetails/?id=3693624471) as "in-memory" mod

To make it run in both builds i separated build specific code in `SharedProject\BuildSpecific` and added one part to `SharedProject\SharedProject.projitems` and the other to `ModConfig.xml`