using System;
using System.Collections.Generic;
using System.Linq;
using SSync.Sockets;
using Symbioz.Protocol.Enums;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Helpers;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Network;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Monsters;

namespace Symbioz.World.Handlers.RolePlay.Commands.Brokers.Fights {
    public class DropsCmdBroker {
        public static void Run(string value, WorldClient client) {
            if (value == null) {
                ShowHelp(client);
                return;
            }

            var split = value.Trim().Split(' ');

            try {
                // @formatter:off
                switch (split[0]) {
                    case "isdroppable": IsDroppable(client, split); break;
                    case "id": IsDroppable(client, split); break;
                    case "adddrop": AddDrop(client, split); break;
                    case "ad": AddDrop(client, split); break;
                    case "rmdrop": RemoveDrop(client, split); break;
                    case "rm": RemoveDrop(client, split); break;
                    case "rd": RemoveDrop(client, split); break;
                    case "updatedrop": UpdateDrop(client, split); break;
                    case "ud": UpdateDrop(client, split); break;
                    default: ShowHelp(client); break;
                }
                // @formatter:on
            }
            catch (Exception e) {
                client.Character.ReplyError($"Command execution failed. ({e.GetType()}).");
                ShowHelp(client);
            }
        }

        private static void IsDroppable(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Find monsters on which an item is droppable");
                client.Character.Reply("» .drops isdroppable|id $ItemId");
                client.Character.Reply(" - <b>$ItemId</b> ⇒ The ID of the item.");

                return;
            }

            ushort itemId = ushort.Parse(args[1]);

            int count = 0;
            ItemRecord item = ItemRecord.GetItem(itemId);
            client.Character.Reply($"Finding drops of item '<b>{item.Name}</b>':");
            foreach (MonsterRecord monster in MonsterRecord.Monsters) {
                foreach (MonsterDrop drop in monster.Drops.Where(drop => drop.ItemId == itemId)) {
                    client.Character.Reply($" - <b>{monster.Name}</b> ({monster.Id}): {drop.PercentDropForGrade1}% "
                                           + $"({drop.PercentDropForGrade2}, {drop.PercentDropForGrade3}, {drop.PercentDropForGrade4}, {drop.PercentDropForGrade5}), "
                                           + $"{drop.ProspectingLock} pp, min={drop.Count}, max={drop.DropLimit}.");
                    count++;
                    break;
                }
            }

            client.Character.Reply($"Drop found on {count} monsters.");
        }

        private static void AddDrop(WorldClient client, string[] args) {
            if (args.Length < 4) {
                client.Character.Reply("Add a drop to a set of monsters");
                client.Character.Reply("» .drops adddrop|ad $ItemId $DropRateG1[,$G2,$G3,G4,G5] $MinPP $MinCount $MaxCount [$MonsterId ...]");
                client.Character.Reply(" - <b>$ItemId</b> ⇒ The ID of the item.");
                client.Character.Reply(" - <b>$DropRateG1</b> ⇒ The drop percent [0, 100]. ");
                client.Character.Reply(" - <b>$G2,3,4,5</b> ⇒ The drop percent [0, 100] for each grade. ");
                client.Character.Reply(" - <b>$MinPP</b> ⇒ The min PP value0.");
                client.Character.Reply(" - <b>$MinCount</b> ⇒ Minimum number of dropped items.");
                client.Character.Reply(" - <b>$MaxCount</b> ⇒ Maximum number of dropped items.");
                client.Character.Reply(" - <b>$MonsterId</b> ⇒ The ID of the monster.");

                return;
            }

            ushort itemId = ushort.Parse(args[1]);
            string rates = args[2];
            int pp = int.Parse(args[3]);
            int minCount = int.Parse(args[4]);
            int maxCount = int.Parse(args[5]);

            string[] ratesSplit = rates.Split(',');
            short g1 = short.Parse(ratesSplit[0]);
            short g2 = (short) (g1 + 2);
            short g3 = (short) (g1 + 4);
            short g4 = (short) (g1 + 6);
            short g5 = (short) (g1 + 8);
            
            if (ratesSplit.Length == 5) {
                g2 = short.Parse(ratesSplit[1]);
                g3 = short.Parse(ratesSplit[2]);
                g4 = short.Parse(ratesSplit[3]);
                g5 = short.Parse(ratesSplit[4]);
            }


            ItemRecord item = ItemRecord.GetItem(itemId);

            int nextDropId = MonsterRecord.GetNextDropId();

            for (int i = 6; i < args.Length; i++) {
                ushort monsterId = ushort.Parse(args[i]);

                MonsterRecord monster = MonsterRecord.GetMonster(monsterId);

                MonsterDrop drop = new MonsterDrop() {
                    ItemId = itemId,
                    Count = minCount,
                    DropId = nextDropId,
                    DropLimit = maxCount,
                    HasCriteria = false,
                    PercentDropForGrade1 = g1,
                    PercentDropForGrade2 = g2,
                    PercentDropForGrade3 = g3,
                    PercentDropForGrade4 = g4,
                    PercentDropForGrade5 = g5,
                    ProspectingLock = pp
                };

                monster.Drops.Add(drop);

                // TODO: check if necessary.
                monster.UpdateInstantElement();

                client.Character.Reply($"Added drop '{item.Name}' ({itemId}) to monster '{monster.Name}' ({monsterId}) with drop rates {g1},{g2},{g3},{g4},{g5} (DropId={nextDropId}).");
                nextDropId++;
            }
        }

        private static void RemoveDrop(WorldClient client, string[] args) {
            if (args.Length < 2) {
                client.Character.Reply("Remove a drop from a monster");
                client.Character.Reply("» .drops rmdrop|rm|rd $ItemId $MonsterId");
                client.Character.Reply("- <b>$ItemId</b> ⇒ The ID of the item.");
                client.Character.Reply("- <b>$MonsterId</b> ⇒ The ID of the monster.");
                return;
            }

            ushort itemId = ushort.Parse(args[1]);
            ItemRecord item = ItemRecord.GetItem(itemId);

            if (args.Length != 3) {
                int count = 0;
                foreach (MonsterRecord monster in MonsterRecord.Monsters) {
                    MonsterDrop drop = monster.Drops.Find(d => d.ItemId == itemId);
                    if (drop != null) {
                        monster.Drops.Remove(drop);
                        count++;
                        client.Character.Reply($" - Removed drop from monster {monster.Name} ({monster.Id}).");

                        monster.UpdateInstantElement();
                    }
                }

                client.Character.Reply($"Removed drop '{item.Name}' ({item.Id}) from {count} monsters.");
            }
            else {
                ushort monsterId = ushort.Parse(args[2]);

                MonsterRecord monsterRecord = MonsterRecord.GetMonster(monsterId);
                monsterRecord.Drops.RemoveAll(d => d.ItemId == itemId);

                monsterRecord.UpdateInstantElement();

                client.Character.Reply($"Successfully removed drop '{item.Name}' ({item.Id}) from monster '{monsterRecord.Name}' {monsterId}");
            }
        }

        private static void UpdateDrop(WorldClient client, string[] args) {
            if (args.Length < 6) {
                client.Character.Reply("Add a drop to a set of monsters");
                client.Character.Reply("» .drops updatedrop|ud $ItemId $MonsterId $DropRate $MinPP $MinCount $MaxCount");
                client.Character.Reply(" - <b>$ItemId</b> ⇒ The ID of the item.");
                client.Character.Reply(" - <b>$MonsterId</b> ⇒ The ID of the item.");
                client.Character.Reply(" - <b>$DropRate</b> ⇒ The drop percent [0, 100].");
                client.Character.Reply(" - <b>$MinPP</b> ⇒ The min PP value0.");
                client.Character.Reply(" - <b>$MinCount</b> ⇒ Minimum number of dropped items.");
                client.Character.Reply(" - <b>$MaxCount</b> ⇒ Maximum number of dropped items.");

                return;
            }

            ushort itemId = ushort.Parse(args[1]);
            ushort monsterId = ushort.Parse(args[2]);
            short dropRate = short.Parse(args[3]);
            int pp = int.Parse(args[4]);
            int minCount = int.Parse(args[5]);
            int maxCount = int.Parse(args[6]);

            ItemRecord item = ItemRecord.GetItem(itemId);
            MonsterRecord monsterRecord = MonsterRecord.GetMonster(monsterId);
            MonsterDrop monsterDrop = monsterRecord.Drops.Find(d => d.ItemId == itemId);

            monsterDrop.PercentDropForGrade1 = dropRate;
            monsterDrop.PercentDropForGrade2 = (short) (dropRate + 2);
            monsterDrop.PercentDropForGrade3 = (short) (dropRate + 4);
            monsterDrop.PercentDropForGrade4 = (short) (dropRate + 6);
            monsterDrop.PercentDropForGrade5 = (short) (dropRate + 8);
            monsterDrop.Count = minCount;
            monsterDrop.DropLimit = maxCount;
            monsterDrop.ProspectingLock = pp;

            monsterRecord.UpdateInstantElement();

            client.Character.Reply($"Successfully removed drop '{item.Name}' ({item.Id}) from monster '{monsterRecord.Name}' {monsterId}");
        }

        public static void ShowHelp(WorldClient client) {
            client.Character.Reply("Manage Drops.");
            client.Character.Reply("» .drops ⇒ Show this help.");
            client.Character.Reply("» .drops isdroppable|id ⇒ Find if an item is dropable.");
            client.Character.Reply("» .drops adddrop|ad ⇒ Add a drop to a set of monsters.");
            client.Character.Reply("» .drops rmdrop|rm|rd ⇒ Remove drop from a set of monsters.");
            client.Character.Reply("» .drops updatedrop|ud ⇒ Remove drop from a set of monsters.");
        }
    }
}