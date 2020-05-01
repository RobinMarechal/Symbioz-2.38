using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Symbioz.Core;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives.Navigation;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Network;
using Symbioz.World.Records.Interactives;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives {
    public class ElementsCmdBroker {
        private const short SKILL_AUTO = -1;

        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            var split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "add": AddElement(client, split); break;
                    case "show": ShowElements(client, split); break;
                    case "s": ShowElements(client, split); break; // alias
                    case "addcrafts": AddCraftTable(client, split); break;
                    case "ac": AddCraftTable(client, split); break; // alias
                    case "addpaddock": AddPaddock(client, split); break;
                    case "mineway": AddMineWay(client, split); break;
                    case "mw": AddMineWay(client, split); break; // alias
                    case "jobcollects": ShowCollectiblesList(client, split); break;
                    case "jc": ShowCollectiblesList(client, split); break;
                    case "addall": AddAllInteractivesOfGfx(client, split); break;
                    case "aa": AddAllInteractivesOfGfx(client, split); break;
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }

        public static void ShowElements(WorldClient client, string[] args) {
            Color[] colors = {
                Color.Blue, Color.Cyan, Color.Yellow, Color.Pink, Color.Goldenrod, Color.Green, Color.Red,
                Color.Purple, Color.Silver, Color.SkyBlue, Color.Black, Color.Brown, Color.Chartreuse, Color.Chocolate,
                Color.Indigo, Color.DarkOliveGreen, Color.LightSlateGray, Color.Navy
            };

            InteractiveElementRecord[] elements =
                InteractiveElementRecord.GetAllElements(client.Character.Map.Id).ToArray();

            if (!elements.Any()) {
                client.Character.Reply("No Element on Map...");
                return;
            }

            if (elements.Length > colors.Length) {
                client.Character.ReplyError($"WARNING: This command will display more elements ({elements.Length}) while there are {colors.Length} different colors.. Be careful.");
            }

            client.Send(new DebugClearHighlightCellsMessage());
            for (int i = 0; i < elements.Count(); i++) {
                InteractiveElementRecord ele = elements[i];
                Color color = colors[i % colors.Length];
                client.Send(new DebugHighlightCellsMessage(color.ToArgb(), new[] { ele.CellId }));
                client.Character.Reply("Element > " + ele.ElementId + " CellId > " + ele.CellId + " GfxId > " + ele.GfxId + " GfxLookId > " + ele.GfxBonesId, color);
            }
        }

        public static void AddCraftTable(WorldClient client, string[] args) {
            if (args.Length < 3) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Add Craft Table.");
                client.Character.Reply("» .elements addcrafts $JobId[:$SkillId] $ElementId [$ElementId ...] ");
                client.Character.Reply(" - <b>$JobId</b> ⇒ The ID of the required job (See your job list in game).");
                client.Character.Reply(" - <b>$SkillId</b> ⇒ (Opt) The ID of related skill. Required if a job has multiple skills (collecting jobs for ex.).");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements) which is supposed to be craft table.");

                return;
            }

            var jobSplit = args[1].Split(':');
            int jobId = int.Parse(jobSplit[0]);
            short skillid = jobSplit.Length > 1 ? short.Parse(jobSplit[1]) : SKILL_AUTO;

            for (int i = 2; i < args.Length; i++) {
                int elementId = int.Parse(args[i]);
                ushort skillRecordId = skillid != SKILL_AUTO ? (ushort) skillid : SkillRecord.Skills.Find(rec => rec.ParentJobId == jobId).Id;

                CreateElement(client, elementId, skillRecordId, "Craft", "", "");
            }

            client.Character.Reply($"Successfully added {args.Length - 2} craft tables for job {(JobsTypeEnum) jobId}.");
        }

        public static void AddPaddock(WorldClient client, string[] args) {
            if (args.Length != 2) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Add an a Paddock.");
                client.Character.Reply("» .elements addpaddock $ElementId ");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements) which is supposed to be a Paddock.");

                return;
            }

            int elementId = int.Parse(args[1]);
            CreateElement(client, elementId, 175, "Enclos", "", ""); // 175 = Access paddock
        }

        public static void AddElement(WorldClient client, string[] args) {
            if (args.Length < 4 || args.Length > 6) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Add an Interactive Element.");
                client.Character.Reply("» .elements add $ElementId $SkillId $ActionType [$Value1 [$Value2]]");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements).");
                client.Character.Reply(" - <b>$SkillId</b> ⇒ The SkillId. Default is 339 (Exit), Stairs=114, .");
                client.Character.Reply(" - <b>$ActionType</b> ⇒ The ActionType string. Ex: Paddock, Teleport, Craft... See InteractiveActionsProvider class.");
                client.Character.Reply(" - <b>$Value1</b> ⇒ (Opt) The InteractiveSkill Value1 column.");
                client.Character.Reply(" - <b>$Value2</b> ⇒ (Opt) The InteractiveSkill Value2 column.");

                return;
            }

            int elementId = int.Parse(args[1]);
            ushort skillId = ushort.Parse(args[2]);
            string actionType = args[3];
            string value1 = args.Length > 4 ? args[4] : "";
            string value2 = args.Length > 5 ? args[5] : "";

            CreateElement(client, elementId, skillId, actionType, value1, value2);
        }

        public static void AddMineWay(WorldClient client, string[] args) {
            if (args.Length < 2) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Add a mine way.");
                client.Character.Reply("» .elements|el mineway|mw $ElementId");
                client.Character.Reply(" - <b>$ElementId</b> ⇒ The element id (see .elements).");

                return;
            }
            
            int elementId = int.Parse(args[1]); // [0] = command name
            int spawnCellId = client.Character.CellId;
            int elementType = 282;
            ushort skillId = 339;

            int currentMapId = client.Character.Map.Id;

            int exitsCount = LinkItem.Exits.Count;
            int entriesCount = LinkItem.Entries.Count;


            if (exitsCount != 0 && entriesCount <= 1) {
                client.Character.ReplyError($"Cannot perform action, Entries and Exits lists must be empty. Entries: {entriesCount}, Exits: {exitsCount}");
                return;
            }

            if (exitsCount == 0 && entriesCount == 0) {
                LinkItem.InitEntry(currentMapId, elementId, elementType, skillId, spawnCellId);
                client.Character.Reply("Added mineway. Register another to link them.");
            }
            else {
                LinkItem entryItem = LinkItem.Entries.First();

                // On the map <mapid>, the element <elementid> which is a <elementtype>, will "Teleport" you if you Use (=114) it, to the map <mapid> on the cell <cellid>
                NewElementData entryData = new NewElementData(entryItem.MapId, entryItem.ElementId, entryItem.ElementType, "Teleport", entryItem.SkillId, currentMapId.ToString(), spawnCellId.ToString());
                // On the map <mapid>, the element <elementid> which is a <elementtype>, will "Teleport" you if you walk on it (as it's a exit =339), to the map <mapid> on the cell <cellid>
                NewElementData exitData = new NewElementData(currentMapId, elementId, elementType, "Teleport", skillId, entryItem.MapId.ToString(), entryItem.SpawnCellId.ToString());

                InteractiveElementsUtils.CreateInteractiveElement(entryData, client);
                InteractiveElementsUtils.CreateInteractiveElement(exitData, client);

                LinkItem.Entries.Clear();
                client.Character.Reply($"Successfully linked maps {entryItem.MapId} and {currentMapId}.");
            }
        }

        public static void ShowCollectiblesList(WorldClient client, string[] args) {
            int jobId = int.Parse(args[1]);

            List<ushort> interactives = SkillRecord.Skills
                                                   .FindAll(s => s.ParentJobId == jobId && s.Name == "Collecter")
                                                   .ConvertAll(skill => skill.InteractiveId);

            interactives.Sort();

            client.Character.Reply($"Collectible resources for job {jobId} ({interactives.Count}): ");
            foreach (var resourceId in new HashSet<ushort>(interactives)) {
                var interactiveRecord = InteractiveRecord.GetInteractive(resourceId);
                client.Character.Reply($" - {interactiveRecord.Name}: {interactiveRecord.Id}");
            }
        }

        private static void AddAllInteractivesOfGfx(WorldClient client, string[] args) {
            if (args.Length < 4) {
                if (args.Length != 1) {
                    client.Character.ReplyError("Invalid command.");
                }

                client.Character.Reply("Add an all interactive elements of specific GfxLookId.");
                client.Character.Reply("» .elements addall|aa $GfxLookId $ElementType $SkillId");
                client.Character.Reply("» Example: .elements addall 685 108 154  ");
                client.Character.Reply(" - <b>$GfxLookId</b> ⇒ The element GxfLookId (see <i>.elements show</i>)");
                client.Character.Reply(" - <b>$ElementType</b> ⇒ ElementType (id in interactives table) of the resource.");
                client.Character.Reply(" - <b>$SkillId</b> ⇒ The ID of the skill.");

                return;
            }
            
            int gfxLookId = int.Parse(args[1]);
            int elementType = int.Parse(args[2]);
            ushort skillId = ushort.Parse(args[3]);

            const string actionType = "Collect";

            int nextUid = InteractiveSkillRecord.InteractiveSkills.DynamicPop(x => x.UID);

            client.Character.Reply($"Retrieving element records with GfxLookId={gfxLookId}...");
            List<InteractiveElementRecord> elementRecords = InteractiveElementRecord.GetElementByGfxLookId(gfxLookId);

            int counter = 1;
            int total = elementRecords.Count;
            HashSet<int> impactedMapIds = new HashSet<int>();

            client.Character.Reply($"Updating {elementRecords.Count} element records...");
            foreach (InteractiveElementRecord record in elementRecords) {
                // Update InteractiveElement
                if(record.ElementType != elementType){
                    record.ElementType = elementType;
                    record.UpdateInstantElement();
                }
                
                client.Character.Reply($"Updated {counter}/{total} InteractiveElement.");

                // Insert InteractiveSkill if not exists
                if (!InteractiveSkillRecord.InteractiveSkills.Exists(isk => isk.ElementId == record.ElementId && isk.SkillId == skillId && isk.ActionType.Equals(actionType))) {
                    InteractiveSkillRecord newRecord = new InteractiveSkillRecord(nextUid, actionType, "", "", record.ElementId, skillId);
                    // InteractiveSkillRecord.InteractiveSkills.Add(newRecord);
                    newRecord.AddInstantElement();
                    nextUid++;
                    
                    client.Character.Reply($"Created {counter}/{total} InteractiveSKill.");
                }
                else {
                    client.Character.Reply($"InteractiveSKill {counter}/{total} Already exists.");
                }

                impactedMapIds.Add(record.MapId);
                counter++;
            }

            client.Character.Reply("Database updated.");

            counter = 1;
            total = impactedMapIds.Count;
            client.Character.Reply($"Reloading {impactedMapIds.Count} maps...");
            foreach (int mapId in impactedMapIds) {
                MapRecord.GetMap(mapId).Instance.Reload();
                client.Character.Reply($"Map {counter}/{total} reloaded: {mapId}");
                counter++;
            }

            client.Character.Reply("All maps have been reloaded.");

            client.Character.Reply("Done!");
        }


        private static void CreateElement(WorldClient client, int elementId, ushort skillId, string actionType, string value1, string value2) {
            SkillRecord skill = SkillRecord.Skills.Find(rec => rec.Id == skillId);
            ushort elementType = skill.InteractiveId;

            InteractiveElementsUtils.CreateInteractiveElement(new NewElementData(client.Character.Map.Id, elementId, elementType, actionType, skillId, value1, value2), client);

            client.Character.Reply($"Element <i>{actionType}</i> Successfully added with skill <i>{skill.Name}</i>.");
        }

        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage Interactive Elements.");
            client.Character.Reply("» .elements ⇒ Show this help.");
            client.Character.Reply("» .elements show ⇒ Show elements on this map.");
            client.Character.Reply("» .elements add ⇒ Add an element.");
            client.Character.Reply("» .elements addcrafts|ac ⇒ Add Craft Tables.");
            client.Character.Reply("» .elements addpaddock|ap ⇒ Add a paddock.");
            client.Character.Reply("» .elements mineway|mw ⇒ Add a mine way.");
            client.Character.Reply("» .elements jobcollects|jc ⇒ Show all collectable elements for a job.");
            client.Character.Reply("» .elements addall|aa ⇒ Configure all interactive element instances for a GfxLookId.");
        }
    }
}