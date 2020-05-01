using System;
using System.Runtime.CompilerServices;
using Symbioz.World.Handlers.RolePlay.Commands.Utils.Mines;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives {
    public class MineCmdBroker {
        public static Mine Mine;

        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            var split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "addmap": AddMap(client, split); break;
                    case "am": AddMap(client, split); break;
                    case "addore": AddOre(client, split); break;
                    case "ao": AddOre(client, split); break;
                    case "init": Init(client, split); break; // alias
                    case "i": Init(client, split); break; // alias
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }

        public static void AddMap(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply(".mine addmap|am [$ElementId ...]");
                client.Character.Reply(".mine <b>$ElementId</b> ⇒ The ID of the elements (see <i>.el show</i>) on your map.");
                return;
            }

            var mapId = client.Character.Map.Id;

            var mineMap = Mine.Maps.Find(map => map.MapId == mapId);
            if (mineMap == null) {
                mineMap = new MineMap(mapId);
                Mine.AddMap(ref mineMap);
            }

            for (int i = 1; i < args.Length; i++) {
                int elementId = int.Parse(args[i]);
                mineMap.AddElement(elementId);
            }

            client.Character.Reply($"Added {args.Length - 1} mine elements to map {mapId}.");
        }

        public static void AddOre(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply(".mine addore|ao [$InteractiveId[:$Quantity=1] ...]");
                client.Character.Reply(".mine <b>$InteractiveId</b> ⇒ The ID in the interactives table.");
                client.Character.Reply(".mine <b>$Quantity</b> ⇒ The quantity. Default is 1");
                return;
            }

            for (var i = 1; i < args.Length; i++) {
                var split = args[i].Split(':');
                
                int interactiveId = int.Parse(split[0]);
                int quantity = split.Length == 2 ? int.Parse(split[1]) : 1;
                
                Mine.AddOre(interactiveId, quantity);
            }
        }

        public static void Init(WorldClient client, string[] args) {
            Mine = new Mine();
            client.Character.Reply("New mine has been created.");
        }


        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage mines.");
            client.Character.Reply("» .mine ⇒ Show this help.");
            client.Character.Reply("» .mine addmap|am ⇒ Show this help.");
            client.Character.Reply("» .mine addore|ao⇒ Show this help.");
            client.Character.Reply("» .mine init|i⇒ Show this help.");
        }
    }
}