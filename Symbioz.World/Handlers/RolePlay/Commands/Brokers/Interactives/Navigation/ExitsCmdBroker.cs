using System;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives.Navigation {
    class ExitsCmdBroker {
        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            var split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "add": AddExit(client, split); break;
                    case "list": ListExits(client); break;
                    case "clear": ClearExits(client); break;
                    case "remove": RemoveExit(client, split); break;
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }
        
        public static void ListExits(WorldClient client) {
            client.Character.Reply("Initialized exits: ");
            foreach (var exit in LinkItem.Exits) {
                client.Character.Reply($" - {exit}");
            }
        }

        public static void RemoveExit(WorldClient client, string[] args) {
            int elementId = int.Parse(args[1]);
            int mapId = args.Length >= 3 ? int.Parse(args[2]) : client.Character.Map.Id;

            if (!LinkItem.Exits.Exists(e => e.ElementId == elementId && e.MapId == mapId)) {
                client.Character.ReplyError($"No registered exit with ElementId={elementId} and MapId={mapId}.");

                return;
            }

            var exit = LinkItem.Exits.Find(e => e.ElementId == elementId && e.MapId == mapId);
            LinkItem.Exits.Remove(exit);

            client.Character.Reply($"Successfully removed exit {exit}.");
        }

        public static void ClearExits(WorldClient client) {
            int count = LinkItem.Exits.Count;
            LinkItem.Exits.Clear();
            client.Character.Reply($"Cleared {count} exits.");
        }

        public static void AddExit(WorldClient client, string[] args) {
            if (args.Length < 2 || args.Length > 4) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }
                
                client.Character.Reply("Create an exit for a link between two maps. Related cmds: .addentry, .link");
                client.Character.Reply("NOTE: Place your character on the cell where you'll want to spawn using the linked entry.");
                client.Character.Reply("» .exits add $ElementId [$ElementType=282 $SkillId=339]");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements).");
                client.Character.Reply(" - <b>$ElementType</b> ⇒ The ElementType value. Ex: 282: exit cell, 284: stair...See 'Interactives' table for more.");
                client.Character.Reply(" - <b>$SkillId</b> ⇒ The SkillId. Default is 339 (Exit), For stairs, use 114.");

                return;
            }

            int spawnCellId = client.Character.CellId;
            int elementId = int.Parse(args[1]);

            int elementType = 282; // Exit cell
            ushort skillId = 339; // Exit skill

            if (args.Length > 3) {
                elementType = int.Parse(args[2]);
                skillId = ushort.Parse(args[3]);
            }

            var exit = LinkItem.InitExit(client.Character.Map.Id, elementId, elementType, skillId, spawnCellId);

            client.Character.Reply("Temporary exit created:");
            client.Character.Reply(exit);
        }

        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage exits.");
            client.Character.Reply("» .exits list ⇒ list all registered exits.");
            client.Character.Reply("» .exits clear ⇒ clear exits list.");
            client.Character.Reply("» .exits add ⇒ add an exit.");
            client.Character.Reply("» .exits remove ⇒ remove an exit");
        }
    }
}