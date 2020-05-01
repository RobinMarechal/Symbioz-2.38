using System.Linq;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives.Navigation {
    public class LinksCmdBroker {
        public static void Run(string value, WorldClient client) {
            if (value == null) {
                client.Character.Reply("Link two maps using a previously created entry and exit. Related cmds: .addentry, .addexit");
                client.Character.Reply("» .link auto ⇒ Works if there's exactly one entry and one exit.");
                client.Character.Reply("» .link $EntryElementId $ExitElementId");
                client.Character.Reply(" - <b>$EntryElementId</b> ⇒ The ElementId of the entry.");
                client.Character.Reply(" - <b>$ExitElementId</b> ⇒ The ElementId of the exit.");

                return;
            }

            LinkItem entryItem;
            LinkItem exitItem;

            var split = value.Trim().Split(' ');

            if (split.Length >= 1 && split[0].ToLower().Equals("auto")) {
                if (LinkItem.Entries.Count == 1 && LinkItem.Exits.Count == 1
                    || LinkItem.Entries.Count > 0 && LinkItem.Exits.Count > 0 && split.Length > 1 && split[1].ToLower().Equals("-f")) {
                    entryItem = LinkItem.Entries.First();
                    exitItem = LinkItem.Exits.First();
                }
                else {
                    client.Character.ReplyError($"Can't create auto link. Entries: {LinkItem.Entries.Count}, Exits: {LinkItem.Exits.Count}.");
                    return;
                }
            }
            else {
                if (split.Length != 2) {
                    client.Character.ReplyError("Invalid command.");
                    return;
                }

                int entryId = int.Parse(split[0]);
                int exitId = int.Parse(split[1]);

                if (!LinkItem.Entries.Exists(e => e.ElementId == entryId)) {
                    client.Character.ReplyError($"Error: Entry with ElementId={entryId} does not exist. Maybe you haven't created it yet? Check cmd .addentry.");

                    return;
                }

                if (!LinkItem.Exits.Exists(e => e.ElementId == exitId)) {
                    client.Character.ReplyError($"Error: Exit with ElementId={exitId} does not exist. Maybe you haven't created it yet? Check cmd .addentry.");

                    return;
                }


                entryItem = LinkItem.Entries.Find(e => e.ElementId == entryId);
                exitItem = LinkItem.Exits.Find(e => e.ElementId == exitId);
            }

            // On the map <mapid>, the element <elementid> which is a <elementtype>, will "Teleport" you if you Use (=114) it, to the map <mapid> on the cell <cellid>
            var entryElementData = new NewElementData(entryItem.MapId, entryItem.ElementId, entryItem.ElementType, "Teleport", entryItem.SkillId, exitItem.MapId.ToString(), exitItem.SpawnCellId.ToString());
            // On the map <mapid>, the element <elementid> which is a <elementtype>, will "Teleport" you if you walk on it (as it's a exit =339), to the map <mapid> on the cell <cellid>
            var exitElementData = new NewElementData(exitItem.MapId, exitItem.ElementId, exitItem.ElementType, "Teleport", exitItem.SkillId, entryItem.MapId.ToString(), entryItem.SpawnCellId.ToString());

            InteractiveElementsUtils.CreateInteractiveElement(entryElementData, client);
            InteractiveElementsUtils.CreateInteractiveElement(exitElementData, client);


            client.Character.Reply($"Successfully linked maps {entryItem.MapId} and {exitItem.MapId}.");

            LinkItem.Entries.Remove(entryItem);
            LinkItem.Exits.Remove(exitItem);
        }
    }
}