using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Symbioz.Core;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Models.Entities;
using Symbioz.World.Network;
using Symbioz.World.Providers.Maps.Npcs;
using Symbioz.World.Records;
using Symbioz.World.Records.Interactives;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Npcs;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives {
    public class NpcCmdBroker {
        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            string[] split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "mapinfo": NpcsInfo(client, split); break;
                    case "map": NpcsInfo(client, split); break;
                    case "mi": NpcsInfo(client, split); break;
                    case "spawn": SpawnNpc(client, split); break;
                    case "s": SpawnNpc(client, split); break;
                    case "delete": DeleteNpc(client, split); break;
                    case "d": DeleteNpc(client, split); break;
                    case "messages": NpcMessageList(client, split); break;
                    case "msgs": NpcMessageList(client, split); break;
                    case "replies": NpcReplyList(client, split); break;
                    case "r": NpcReplyList(client, split); break;
                    case "addaction": NpcAddAction(client, split); break;
                    case "aa": NpcAddAction(client, split); break;
                    case "rmaction": NpcRemoveAction(client, split); break;
                    case "ra": NpcRemoveAction(client, split); break;
                    case "addreply": NpcAddReply(client, split); break;
                    case "ar": NpcAddReply(client, split); break;
                    case "rmreply": NpcRemoveReply(client, split); break;
                    case "rr": NpcRemoveReply(client, split); break;
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }

        public static void NpcsInfo(WorldClient client, string[] args) {
            List<NpcSpawnRecord> spawnRecords = NpcSpawnRecord.GetMapNpcs(client.Character.Map.Id);

            client.Character.Reply("Npcs on map:");
            foreach (NpcSpawnRecord spawnRecord in spawnRecords) {
                client.Character.Reply($" - {spawnRecord.Template.Name}: Id={spawnRecord.TemplateId}, SpawnId={spawnRecord.Id}");
            }
        }

        public static void SpawnNpc(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Spawn an NPC on the map. The NPC will have the same direction as you.");
                client.Character.Reply(".npc spawn|s $NpcId [$CellId]");
                client.Character.Reply(" - $NpcId ⇒ ID of the NPC you want to spawn.");
                client.Character.Reply(" - $CellId ⇒ (Opt) The targett cell. Default is your current cell.");

                return;
            }

            Character ch = client.Character;

            ushort npcId = ushort.Parse(args[1]);
            ushort cellId = args.Length > 2 ? ushort.Parse(args[2]) : ch.CellId;
            MapRecord mapRecord = ch.Map;
            sbyte recordDirection = ch.Record.Direction;

            NpcSpawnRecord spawnRecord = NpcSpawnsManager.Instance.Spawn(npcId, mapRecord, cellId, recordDirection);

            client.Character.Map.Instance.Reload();
            ch.Reply($"Spawned npc {spawnRecord.Template.Name} (NpcId={spawnRecord.Template.Id}, SpawnId={spawnRecord.Id})");
        }

        public static void DeleteNpc(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Delete an NPC spawn instance. You must be on the same map.");
                client.Character.Reply(".npc delete|d $SpawnId");
                client.Character.Reply(" - $NpcId ⇒ ID of the NPC you want to spawn.");

                return;
            }

            int spawnId = int.Parse(args[1]);

            Npc npc = client.Character.Map.Instance.GetEntities<Npc>().FirstOrDefault(x => x.SpawnRecord.Id == spawnId);

            if (npc == null) {
                client.Character.Reply($"No NPC spawn found with id {spawnId}. See cmd <i>.npc infos</i>.");
                return;
            }

            npc.SpawnRecord.RemoveInstantElement();
            client.Character.Map.Instance.RemoveEntity(npc);
            client.Character.Map.Instance.Reload();
        }


        public static void NpcMessageList(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Get NPC's available messages.");
                client.Character.Reply(".npc messages|msgs $NpcId");
                client.Character.Reply(" - $NpcId ⇒ ID of the NPC template (not spawn id).");

                return;
            }

            ushort npcTemplateId = ushort.Parse(args[1]);

            NpcRecord npcRecord = NpcRecord.GetNpc(npcTemplateId);
            CSVDoubleArray messagesArray = npcRecord.Messages;

            client.Character.Reply($"Messages for npc {npcRecord.Id} ({messagesArray.Values.Length}): ");

            foreach (uint[] tuple in messagesArray.Values) {
                uint msgId = tuple[0];
                client.Character.Reply($" - {msgId} ");
            }
        }


        public static void NpcReplyList(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Get NPC's available replies.");
                client.Character.Reply(".npc replies|r $NpcId");
                client.Character.Reply(" - $NpcId ⇒ ID of the NPC template (not spawn id).");

                return;
            }

            ushort npcTemplateId = ushort.Parse(args[1]);

            NpcRecord npcRecord = NpcRecord.GetNpc(npcTemplateId);
            CSVDoubleArray messagesArray = npcRecord.Replies;

            client.Character.Reply($"Replies for npc {npcRecord.Id} ({messagesArray.Values.Length}): ");

            foreach (uint[] tuple in messagesArray.Values) {
                uint msgId = tuple[0];
                client.Character.Reply($" - {msgId}");
            }
        }


        public static void NpcAddAction(WorldClient client, string[] args) {
            if (args.Length < 3) {
                client.Character.Reply("Add an action to an NPC.");
                client.Character.Reply(".npc addaction|aa $SpawnId $ActionTypeId $Value1 $Value2");
                client.Character.Reply(" - $SpawnId ⇒ ID of the NPC spawn instance.");
                client.Character.Reply(" - $ActionTypeId ⇒ The action type int. Talk = 3. See NpcActionTypeEnum.");
                client.Character.Reply(" - $Value1 ⇒ The action's value1. The value depends on the action type. For Talk: message ID.");
                client.Character.Reply(" - $Value2 ⇒ The action's value2. The value depends on the action type. For Talk: empty. ");

                return;
            }

            int spawnId = int.Parse(args[1]);
            sbyte actionId = sbyte.Parse(args[2]); // Talk = 3
            string value1 = args.Length > 3 ? args[3] : ""; // Talk ? MessageId
            string value2 = args.Length > 4 ? args[4] : "";

            NpcActionRecord npcAction = new NpcActionRecord(spawnId, actionId, value1, value2);
            npcAction.AddInstantElement();

            Npc npc = client.Character.Map.Instance.GetNpc(spawnId);
            npc.ActionsRecord.Add(npcAction);

            client.Character.Map.Instance.Reload();
            client.Character.Reply($"Successfully added action {actionId} (Value1='{value1}', Value2='{value2}') to npc SpawnId={spawnId}.");
        }


        public static void NpcRemoveAction(WorldClient client, string[] args) {
            if (args.Length < 3) {
                client.Character.Reply("Remove an action from an NPC.");
                client.Character.Reply(".npc rmaction|ra $SpawnId $ActionTypeId");
                client.Character.Reply(" - $SpawnId ⇒ ID of the NPC spawn instance.");
                client.Character.Reply(" - $ActionTypeId ⇒ The action type int. Talk = 3. See NpcActionTypeEnum.");

                return;
            }

            int spawnId = int.Parse(args[1]);
            sbyte actionId = sbyte.Parse(args[2]);

            Npc npc = client.Character.Map.Instance.GetNpc(spawnId);

            NpcActionRecord record = npc.ActionsRecord.Find(r => r.NpcId == spawnId && r.ActionId == actionId);
            record.RemoveInstantElement();

            npc.ActionsRecord.Remove(record);

            client.Character.Map.Instance.Reload();
            client.Character.Reply($"Successfully removed action {actionId} (Value1='{record.Value1}', Value2='{record.Value2}') for npc SpawnId={spawnId}.");
        }


        public static void NpcAddReply(WorldClient client, string[] args) {
            if (args.Length < 3) {
                client.Character.Reply("Bind a reply to an NPC message.");
                client.Character.Reply(".npc addreply|ar $MessageId $ReplyId $ActionType $Value1 $Value2 $Condition ");
                client.Character.Reply(" - $MessageId ⇒ The ID of the message.");
                client.Character.Reply(" - $ReplyId ⇒ The ID of the reply.");
                client.Character.Reply(" - $ActionType ⇒ The type of action (eg. Teleport, AddItem, RemoveItem... See InteractiveActionsProvider).");
                client.Character.Reply(" - $Value1 ⇒ The action's value1, depending on the type. If Teleport: the destination Map ID. If Add|RemoveItem: The Item ID.");
                client.Character.Reply(" - $Value2 ⇒ The action's value2, depending on the type. If Teleport: the destination cell ID. If Add|RemoveItem: The quantity.");
                client.Character.Reply(" - $Condition ⇒ The condition for the action.");

                return;
            }

            ushort messageId = ushort.Parse(args[1]);
            ushort replyId = ushort.Parse(args[2]);
            string actionType = args.Length > 3 ? args[3] : "";
            string value1 = args.Length > 4 ? args[4] : "";
            string value2 = args.Length > 5 ? args[5] : "";
            string condition = args.Length > 6 ? args[6] : "";

            NpcReplyRecord npcReply = new NpcReplyRecord(messageId, replyId, actionType, value1, value2, condition, "");
            npcReply.AddInstantElement();

            client.Character.Map.Instance.Reload();
            client.Character.Reply($"Successfully added reply {replyId} to message {messageId} (ActionType={actionType}, Value1={value1}, Value2={value2}, Condition={condition}).");
        }


        public static void NpcRemoveReply(WorldClient client, string[] args) {
            ushort messageId = ushort.Parse(args[1]);
            ushort replyId = ushort.Parse(args[2]);

            NpcReplyRecord record = NpcReplyRecord.NpcsReplies.Find(r => r.MessageId == messageId && r.ReplyId == replyId);
            record.RemoveInstantElement();

            // TODO: test if previous statement is enough YEP
            // NpcReplyRecord.NpcsReplies.Remove(record);

            client.Character.Map.Instance.Reload();
            client.Character.Reply($"Successfully removed reply {replyId} of message {messageId} (ActionType={record.ActionType}, Value1={record.Value1}, Value2={record.Value2}, Condition={record.Condition}).");
        }

        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage NPCs.");
            client.Character.Reply("» .npc ⇒ Show this help.");
            client.Character.Reply("» .npc mapinfo|map|mi ⇒ Show current map's NPCs info.");
            client.Character.Reply("» .npc spawn|s ⇒ Spawn an NPC on current map.");
            client.Character.Reply("» .npc delete|d ⇒ Delete an NPC spawn instance.");
            client.Character.Reply("» .npc messages|msgs ⇒ List all available message for an NPC.");
            client.Character.Reply("» .npc replies|r ⇒ List all available replies for an NPC.");
            client.Character.Reply("» .npc addaction|aa ⇒ Bind an action to an NPC spawn instance.");
            client.Character.Reply("» .npc rmaction|ra ⇒ Remove an action to an NPC spawn instance.");
            client.Character.Reply("» .npc addreply|ar ⇒ Add a reply to a NPC message.");
            client.Character.Reply("» .npc rmreply|rr ⇒ Remove a reply from an NPC message.");
        }
    }
}