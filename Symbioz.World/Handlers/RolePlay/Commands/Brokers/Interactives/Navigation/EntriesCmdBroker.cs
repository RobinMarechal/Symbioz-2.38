using System;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives.Navigation {
    class EntriesCmdBroker {
        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            var split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "add": AddEntry(client, split); break;
                    case "list": ListEntries(client); break;
                    case "clear": ClearEntries(client); break;
                    case "remove": RemoveEntry(client, split); break;
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }

        public static void ListEntries(WorldClient client) {
            client.Character.Reply("Initialized entries: ");
            foreach (var entry in LinkItem.Entries) {
                client.Character.Reply($" - {entry}");
            }
        }

        public static void RemoveEntry(WorldClient client, string[] args) {
            int elementId = int.Parse(args[1]);
            int mapId = args.Length >= 3 ? int.Parse(args[2]) : client.Character.Map.Id;

            if (!LinkItem.Entries.Exists(e => e.ElementId == elementId && e.MapId == mapId)) {
                client.Character.ReplyError($"No registered entry with ElementId={elementId} and MapId={mapId}.");

                return;
            }

            var entry = LinkItem.Entries.Find(e => e.ElementId == elementId && e.MapId == mapId);
            LinkItem.Entries.Remove(entry);

            client.Character.Reply($"Successfully removed entry {entry}.");
        }

        public static void ClearEntries(WorldClient client) {
            int count = LinkItem.Entries.Count;
            LinkItem.Entries.Clear();
            client.Character.Reply($"Cleared {count} entries.");
        }

        public static void AddEntry(WorldClient client, string[] args) {
            if (args.Length < 2 || args.Length > 4) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Create an entry for a link between two maps. Related cmds: .exits, .addlink");
                client.Character.Reply("NOTE: Place your character on the cell where you'll want to spawn using the linked exit.");
                client.Character.Reply("» .entries add $ElementId [$ElementType=70 $SkillId=84]");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements)");
                client.Character.Reply(" - <b>$ElementType</b> ⇒ The ElementType value. Ex: 70: door, 284: stair... see 'Interactives' table for more");
                client.Character.Reply(" - <b>$SkillId</b> ⇒ The SkillId. Enter=84, Use=114");

                return;
            }

            int spawnCellId = client.Character.CellId;
            int elementId = int.Parse(args[1]);
            int elementType = 70;
            ushort skillId = 84;

            if (args.Length > 3) {
                elementType = int.Parse(args[2]);
                skillId = ushort.Parse(args[3]);
            }

            var entry = LinkItem.InitEntry(client.Character.Map.Id, elementId, elementType, skillId, spawnCellId);

            client.Character.Reply("Temporary entry created:");
            client.Character.Reply(entry);
        }

        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage entries.");
            client.Character.Reply("» .entries list ⇒ list all registered entries.");
            client.Character.Reply("» .entries clear ⇒ clear entries list.");
            client.Character.Reply("» .entries add ⇒ add an entry.");
            client.Character.Reply("» .entries remove ⇒ remove an entry");
        }
    }
}