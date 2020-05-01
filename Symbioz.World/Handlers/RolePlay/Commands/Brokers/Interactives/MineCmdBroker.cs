using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Symbioz.Core;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Handlers.RolePlay.Commands.Utils.Mines;
using Symbioz.World.Network;
using Symbioz.World.Records.Interactives;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives {
    public class MineCmdBroker {
        // public static Dictionary<int, int> ElementsGfxLookDictionary = new Dictionary<int, int>() {
        //     { 1081, 17}, // Fer: 17
        //     { 1075, 53 }, // Pierre Cuivr├®e: 53
        //     { 1074, 55 }, // Bronze: 55
        //     { 1063, 37 }, // Pierre de Kobalte: 37
        //     { 1078, 54}, // Mangan├¿se: 54
        //     { 52 }, // Etain: 52
        //     { 1080, 114 }, // Silicate: 114
        //     { 1072, 24 }, // Argent: 24 ???
        //     { 26 }, // Pierre de Bauxite: 26
        //     { 25 }, // Or: 25
        //     { 1076, 113 }, // Dolomite: 113
        //     { 1290, 135 } // Obsidienne: 135
        // };

        public static Color[] Colors = {
            Color.Blue, Color.Cyan, Color.Yellow, Color.Pink, Color.Goldenrod, Color.Green, Color.Red,
            Color.Purple, Color.Silver, Color.SkyBlue, Color.Black, Color.Brown, Color.Chartreuse, Color.Chocolate,
            Color.Indigo, Color.DarkOliveGreen, Color.LightSlateGray, Color.Navy
        };

        public static Mine Mine;

        public static int PreSelectionMapId = -1;
        public static List<InteractiveElementRecord> PreSelectionElementsIds;

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
                    case "build": Build(client, split); break; // alias
                    case "preselect": PreSelect(client, split); break; // alias
                    case "ps": PreSelect(client, split); break; // alias
                    case "showpreselect": ShowPreSelect(client, split); break; // alias
                    case "sps": ShowPreSelect(client, split); break; // alias
                    case "exclude": ExcludeElements(client, split); break; // alias
                    case "exc": ExcludeElements(client, split); break; // alias
                    case "save": SavePreSelection(client, split); break; // alias
                    case "print": Print(client, split); break; // alias
                    case "p": Print(client, split); break; // alias
                    case "delete": Delete(client, split); break; // alias
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
            if (Mine == null) {
                client.Character.ReplyError("You must initialize your mine first. See <i>.mine init</i>.");
                return;
            }

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
            if (Mine == null) {
                client.Character.ReplyError("You must initialize your mine first. See <i>.mine init</i>.");
                return;
            }

            if (args.Length < 2) {
                client.Character.Reply(".mine addore|ao [$InteractiveId[:$Quantity=1] ...]");
                client.Character.Reply(".mine <b>$InteractiveId</b> ⇒ The ID in the interactives table.");
                client.Character.Reply(".mine <b>$Quantity</b> ⇒ The quantity. Default is 1");
                return;
            }

            for (var i = 1; i < args.Length; i++) {
                var split = args[i].Split(':');

                ushort interactiveId = ushort.Parse(split[0]);
                int quantity = split.Length == 2 ? int.Parse(split[1]) : 1;

                int total = Mine.AddOre(interactiveId, quantity);

                InteractiveRecord interactiveRecord = InteractiveRecord.GetInteractive(interactiveId);

                client.Character.Reply($"Registered '{interactiveRecord.Name}' ({interactiveId}) x{quantity} (Total: {total})");
            }
        }

        public static void Init(WorldClient client, string[] args) {
            Mine = new Mine();
            client.Character.Reply("New mine has been created.");
        }

        public static void Build(WorldClient client, string[] args) {
            if (Mine == null) {
                client.Character.ReplyError("Unable to create mine, none has been initialized.");
                return;
            }

            if (!Mine.Maps.Any()) {
                client.Character.ReplyError("You have not registered any map. See cmds <i>.mine addmap</i> and <i>.mine ps</i>.");
                return;
            }

            int idx = 0;
            List<int> oreSequence = BuildRandomInteractivesSequence();

            if (!oreSequence.Any()) {
                client.Character.ReplyError("You have not registered any ore. See cmd <i>.mine addore</i>.");
                return;
            }

            int nextSkillUid = InteractiveSkillRecord.InteractiveSkills.DynamicPop(x => x.UID);

            foreach (MineMap mineMap in Mine.Maps) {
                foreach (int elementId in mineMap.ElementIds) {
                    int elementType = oreSequence[idx];

                    // InteractiveElements
                    InteractiveElementRecord interactiveElementRecord = InteractiveElementRecord.InteractiveElements.Find(rec => rec.MapId == mineMap.MapId && rec.ElementId == elementId);
                    interactiveElementRecord.ElementType = elementType;
                    // interactiveElementRecord.UpdateInstantElement();

                    // InteractiveSkills
                    InteractiveSkillRecord interactiveSkillRecord = new InteractiveSkillRecord(nextSkillUid, "Collect", "", "", elementId, (int) JobsTypeEnum.Mineur);
                    InteractiveSkillRecord.InteractiveSkills.Add(interactiveSkillRecord);
                    // interactiveSkillRecord.AddInstantElement();

                    idx++;
                    nextSkillUid++;
                }

                MapRecord mapRecord = MapRecord.GetMap(mineMap.MapId);
                mapRecord.Instance.Reload();

                client.Character.Reply($"Processed and reloaded map {mineMap.MapId}.");
            }

            // Mine = null;
            client.Character.Reply("Successfully built mine.");
        }

        private static List<int> BuildRandomInteractivesSequence() {
            List<int> interactives = new List<int>();

            if (!Mine.Ores.Any()) {
                return interactives;
            }

            foreach (KeyValuePair<int, int> kv in Mine.Ores) {
                int ore = kv.Key;
                int quantity = kv.Value;

                for (var i = 0; i < quantity; i++) {
                    interactives.Add(ore);
                }
            }

            int nbElements = Mine.Maps.ConvertAll(m => m.ElementIds.Count).Sum();
            while (interactives.Count < nbElements) {
                interactives.Add(interactives.Random());
            }

            while (interactives.Count > nbElements) {
                interactives.Remove(interactives.Random());
            }

            return new List<int>(interactives.Shuffle().Shuffle().Shuffle());
        }

        public static void ShowPreSelect(WorldClient client, string[] args) {
            if (client.Character.Map.Id != PreSelectionMapId) {
                client.Character.Reply($"You are not on the map of your pre-selection... ({PreSelectionMapId})");
                return;
            }

            if (!PreSelectionElementsIds.Any()) {
                client.Character.Reply("No Element on Map...");
                return;
            }

            if (PreSelectionElementsIds.Count > Colors.Length) {
                client.Character.ReplyError($"WARNING: This command will display more elements ({PreSelectionElementsIds.Count}) while there are {Colors.Length} different colors.. Be careful.");
            }

            client.Send(new DebugClearHighlightCellsMessage());
            for (int i = 0; i < PreSelectionElementsIds.Count; i++) {
                InteractiveElementRecord ele = PreSelectionElementsIds[i];
                Color color = Colors[i % Colors.Length];
                client.Send(new DebugHighlightCellsMessage(color.ToArgb(), new[] { ele.CellId }));
                client.Character.Reply("Element > " + ele.ElementId + " CellId > " + ele.CellId + " GfxLookId > " + ele.GfxBonesId, color);
            }
        }

        public static void PreSelect(WorldClient client, string[] args) {
            if (Mine == null) {
                client.Character.Reply("No initialized mine.");
                return;
            }

            PreSelectionMapId = client.Character.Map.Id;
            PreSelectionElementsIds = InteractiveElementRecord.GetAllElements(client.Character.Map.Id);

            ShowPreSelect(client, args);

            client.Character.Reply("");
            client.Character.Reply("You can now use cmds: ");
            client.Character.Reply(" - <i>.mine exclude</i> to exclude any elements.");
            client.Character.Reply(" - <i>.mine savemap</i> to save all pre-selected elements on your map.");
        }


        public static void SavePreSelection(WorldClient client, string[] args) {
            if (PreSelectionElementsIds == null) {
                client.Character.ReplyError("Create a pre-selection first using <i>.mine preselect</i>.");
                return;
            }

            var mapId = PreSelectionMapId;

            var mineMap = Mine.Maps.Find(map => map.MapId == mapId);
            if (mineMap == null) {
                mineMap = new MineMap(mapId);
                Mine.AddMap(ref mineMap);
            }

            for (int i = 1; i < PreSelectionElementsIds.Count; i++) {
                int elementId = PreSelectionElementsIds[i].ElementId;
                mineMap.AddElement(elementId);
            }

            client.Character.Reply($"Added {PreSelectionElementsIds.Count - 1} mine elements to map {mapId}.");
        }

        public static void ExcludeElements(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Exclude elements from the pre selection");
                client.Character.Reply(".mine exclude [$ElementId ...]");
                client.Character.Reply(".mine <b>$ElementId</b> ⇒ The ElementId. to exclude");
                return;
            }

            if (PreSelectionMapId == -1) {
                client.Character.ReplyError("Create a pre-selection first using <i>.mine preselect</i>.");
                return;
            }

            if (client.Character.Map.Id != PreSelectionMapId) {
                client.Character.ReplyError($"Cannot exclude Elements, you must be on the map of your pre-selection ({PreSelectionMapId}). Or create a new preselection using cmd <i>.mine preselect</i>.");
                return;
            }

            for (var i = 1; i < args.Length; i++) {
                var toExclude = int.Parse(args[i]);
                InteractiveElementRecord toRemove = PreSelectionElementsIds.Find(element => element.ElementId == toExclude);
                if (toRemove != null) {
                    PreSelectionElementsIds.Remove(toRemove);
                    client.Character.Reply($"Successfully unselected element {toRemove.ElementId}");
                }
                else {
                    client.Character.ReplyError($"No element {toExclude} in the pre-selection.");
                }
            }

            ShowPreSelect(client, null);
        }

        public static void Print(WorldClient client, string[] args) {
            if (Mine == null) {
                client.Character.Reply("No initialized mine.");
                return;
            }

            client.Character.Reply("Printing mine data:");
            client.Character.Reply($"Maps ({Mine.Maps.Count}): ");
            foreach (MineMap mineMap in Mine.Maps) {
                client.Character.Reply($" - {mineMap.MapId} ({mineMap.ElementIds.Count} elements)");
            }

            client.Character.Reply($"Ores ({Mine.Ores.Count} types):");
            foreach (var kv in Mine.Ores) {
                int interactiveId = kv.Key;
                int quantity = kv.Value;

                var interactive = InteractiveRecord.GetInteractive((ushort) interactiveId);

                client.Character.Reply($" - {interactive.Name} ({interactiveId}) x{quantity}");
            }
        }

        public static void Delete(WorldClient client, string[] args) {
            if (Mine == null) {
                client.Character.Reply("No initialized mine. Nothing to delete.");
                return;
            }

            Mine = null;

            client.Character.Reply("Mine successfully deleted. Initialized a new one using <i>.mine init</i>.");
        }


        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage mines.");
            client.Character.Reply("» .mine ⇒ Show this help.");
            client.Character.Reply("» .mine init|i ⇒ Initialize a mine.");
            client.Character.Reply("» .mine addmap|am ⇒ Add a map to the mine.");
            client.Character.Reply("» .mine addore|ao ⇒ Add ores to the mine.");
            client.Character.Reply("» .mine build ⇒ Build the mine when finished.");
            client.Character.Reply("» .mine print|p ⇒ Print mine's information.");
            client.Character.Reply("» .mine preselect|ps ⇒ Highlight and pre-select all mine's map elements information.");
            client.Character.Reply("» .mine showpreselect|sps ⇒ Show preselection.");
            client.Character.Reply("» .mine exclude|exc ⇒ Exclude elements of the pre-selection created using <i>.mine showmap</i> cmd.");
            client.Character.Reply("» .mine save ⇒ Save all highlighted elements on your current map.");
            client.Character.Reply("» .mine delete ⇒ Delete the mine you are building.");
            client.Character.Reply("» .mine delete ⇒ Delete the mine you are building.");
            client.Character.Reply("» .mine buildmines|bm ⇒ Build all mines in the game.");
        }
    }
}