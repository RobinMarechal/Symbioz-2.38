using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Symbioz.Core;
using Symbioz.ORM;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Handlers.RolePlay.Commands.Brokers.Fights;
using Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives;
using Symbioz.World.Handlers.RolePlay.Commands.Brokers.Interactives.Navigation;
using Symbioz.World.Handlers.RolePlay.Commands.Utils;
using Symbioz.World.Models;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Entities.Look;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Modules;
using Symbioz.World.Network;
using Symbioz.World.Providers.Delayed;
using Symbioz.World.Providers.Fights.Results;
using Symbioz.World.Providers.Guilds;
using Symbioz.World.Providers.Maps.Monsters;
using Symbioz.World.Providers.Maps.Npcs;
using Symbioz.World.Records;
using Symbioz.World.Records.Almanach;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Interactives;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Npcs;
using Symbioz.World.Records.Spells;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Symbioz.World.Handlers.RolePlay.Commands {
    class CommandsRepertory {
        [ChatCommand("map", ServerRoleEnum.Administrator)]
        public static void MapInformation(string value, WorldClient client) {
            foreach (Entity entity in client.Character.Map.Instance.GetEntities()) {
                client.Character.Reply(entity.ToString(), Color.CornflowerBlue);
            }
        }


        [ChatCommand("recipe", ServerRoleEnum.Administrator)]
        public static void RecipeCommand(string value, WorldClient client) {
            for (int i = 0; i < 50; i++) {
                RecipeRecord recipe = RecipeRecord.GetRecipe(ushort.Parse(value));

                foreach (var item in recipe.Ingredients) {
                    client.Character.Inventory.AddItem(item.Key, item.Value);
                }
            }
        }


        [ChatCommand("itemlist", ServerRoleEnum.Administrator)]
        public static void ItemListCommand(string value, WorldClient client) {
            ItemTypeEnum itemType = (ItemTypeEnum) int.Parse(value);

            var itemIds = Array.ConvertAll(ItemRecord.GetItems(itemType), x => x.Id);

            client.Character.Reply("Items for type: " + itemType + Environment.NewLine + itemIds.ToCSV());
        }

        
        [ChatCommand("delayedact", ServerRoleEnum.Fondator)]
        public static void CreateDelayedAction(string value, WorldClient client) {
            var split = value.Split(' ');

            string actionType = split[0]; // for dungeon: Monsters
            int interval = int.Parse(split[1]); // ex: 30 (in seconds)
            string value1 = split[2]; // For dungeons: list of monsters
            var value2 = client.Character.Map.Id.ToString();

            DelayedActionRecord record = new DelayedActionRecord(actionType, interval, value1, value2);

            record.AddInstantElement();

            DelayedAction action = new DelayedAction(record);
            DelayedActionManager.AddAction(action);
            action.Execute();

            client.Character.Reply($"Successfully created DelayedAction record {record}.");
        }

        [ChatCommand("delayedactrm", ServerRoleEnum.Fondator)]
        public static void RemoveDelayedAction(string value, WorldClient client) {
            int id = int.Parse(value);

            DelayedActionRecord record = DelayedActionRecord.DelayedActions.Find(r => r.Id == id);
            DelayedActionManager.DelayedActions.RemoveAll(act => act.Record == record);
            record.RemoveInstantElement();

            client.Character.Reply($"Successfully deleted DelayedAction {id}.");
        }


        [ChatCommand("dialog", ServerRoleEnum.Fondator)]
        public static void DialogCommand(string value, WorldClient client) {
            if (client.Character.Dialog != null) {
                client.Character.Reply("Dialog: " + client.Character.Dialog.GetType().Name);
            }

            if (client.Character.RequestBox != null) {
                client.Character.Reply("RequestBox: " + client.Character.RequestBox.GetType().Name);
            }
        }


        [ChatCommand("monsters", ServerRoleEnum.Moderator)]
        public static void MonsterCommand(string value, WorldClient client) {
            if (client.Character.Map.Instance.MonsterGroupCount >= MonsterSpawnManager.MaxMonsterGroupPerMap) {
                client.Character.ReplyError("Impossible d'ajouter un groupe de monstres a la carte, celle-ci est déja complete.");

                return;
            }

            List<MonsterSpawnRecord> spawns = new List<MonsterSpawnRecord>();

            foreach (string monsterId in value.Split(',')) {
                spawns.Add(new MonsterSpawnRecord(MonsterSpawnRecord.MonsterSpawns.DynamicPop(x => x.Id),
                                                  ushort.Parse(monsterId),
                                                  client.Character.Map.SubAreaId,
                                                  100));
            }

            if (spawns.Count > 0)
                MonsterSpawnManager.Instance.AddGeneratedMonsterGroup(client.Character.Map.Instance,
                                                                      spawns.ToArray(),
                                                                      false);
            else
                client.Character.Reply("Specifiez une liste de monstre (id1,id2,id3..etc)");
        }


        [ChatCommand("dj", ServerRoleEnum.Fondator)]
        public static void DjCommand(string value, WorldClient client) {
            if (value == null) {
                client.Character.ReplyError("You must provide a list of monsters to spawn. Example: 23,24,25,26.");
                return;
            }

            int id = DelayedActionRecord.DelayedActions.DynamicPop(x => x.Id);
            DelayedActionRecord record =
                new DelayedActionRecord(id, "Monsters", 30, value, client.Character.Map.Id.ToString());
            record.AddElement();
            DelayedAction action = new DelayedAction(record);
            DelayedActionManager.AddAction(action);
            action.Execute();
        }


        [ChatCommand("reloadAlmanach", ServerRoleEnum.Fondator)]
        public static void Reload(string value, WorldClient client) {
            DatabaseManager.GetInstance().Reload<AlmanachRecord>();
        }


        [ChatCommand("addOrnament", ServerRoleEnum.Animator)]
        public static void AddOrnament(string value, WorldClient client) {
            string[] args = value.Split(' ');
            if (args.Length < 1) {
                client.Character.Reply("Specifiez l'id de l'ornamenent.");

                return;
            }

            if (args.Length == 1) {
                client.Character.LearnOrnament(ushort.Parse(args[0]), true);
            }

            if (args.Length == 2) {
                Character target = WorldServer.Instance.GetOnlineClient(args[0]).Character;
                target.LearnOrnament(ushort.Parse(args[1]), true);
                client.Character.Reply(target.Name + " connais desormais l'ornement " + args[1]);
            }
        }


        [ChatCommand("kick", ServerRoleEnum.Moderator)]
        public static void Kick(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                target.Disconnect();
            }
        }


        [ChatCommand("reloadSubareas", ServerRoleEnum.Fondator)]
        public static void ReloadSubarea(string value, WorldClient client) {
            DatabaseManager.GetInstance().Reload<SubareaRecord>();

            foreach (MapRecord map in MapRecord.Maps) {
                map.SubArea = SubareaRecord.GetSubarea(map.SubAreaId);
            }

            client.Character.Reply("Reloaded");
        }


        [ChatCommand("subarea", ServerRoleEnum.Fondator)]
        public static void GetSubarea(string value, WorldClient client) {
            SubareaRecord subAreaRecord = client.Character.Map.SubArea;
            client.Character.Reply($"Subarea: {subAreaRecord.Name} ({subAreaRecord.Id}).");
        }


        [ChatCommand("reloadmap", ServerRoleEnum.Fondator)]
        public static void ReloadMap(string value, WorldClient client) {
            client.Character.Map.Instance.Reload();
            client.Character.Reply("Current map reloaded.");
        }


        [ChatCommand("addTitle", ServerRoleEnum.Animator)]
        public static void AddTitle(string value, WorldClient client) {
            string[] args = value.Split(' ');
            if (args.Length < 1) {
                client.Character.Reply("Specifiez l'id du titre.");

                return;
            }

            if (args.Length == 1) {
                client.Character.LearnTitle(ushort.Parse(args[0]));
            }

            if (args.Length == 2) {
                Character target = WorldServer.Instance.GetOnlineClient(args[0]).Character;
                target.LearnTitle(ushort.Parse(args[1]));
                client.Character.Reply(target.Name + " possède maintenant le titre " + args[1]);
            }
        }


        [ChatCommand("addEmote", ServerRoleEnum.Animator)]
        public static void AddEmote(string value, WorldClient client) {
            string[] args = value.Split(' ');
            if (args.Length < 1) {
                client.Character.Reply("Specifiez l'id de l'émote.");

                return;
            }

            if (args.Length == 1) {
                client.Character.LearnEmote((byte) int.Parse(args[0]));
            }

            if (args.Length == 2) {
                Character target = WorldServer.Instance.GetOnlineClient(args[0]).Character;
                target.LearnEmote((byte) int.Parse(args[1]));
                client.Character.Reply(target.Name + " possède maintenant l'émote " + EmoteRecord.GetEmote((byte) int.Parse(args[1])).Name);
            }
        }


        [ChatCommand("leaf", ServerRoleEnum.Fondator)]
        public static void Leaf(string value, WorldClient client) {
            client.Character.Fighter.OposedTeam().KillTeam();
        }


        [ChatCommand("kamas", ServerRoleEnum.Moderator)]
        public static void KamasCommand(string value, WorldClient client) {
            int amount = int.Parse(value);
            client.Character.AddKamas(amount);
            client.Character.OnKamasGained(amount);
        }


        [ChatCommand("itemset", ServerRoleEnum.Administrator)]
        public static void ItemSetCommand(string value, WorldClient client) {
            ItemSetRecord set = ItemSetRecord.ItemsSets.Find(x => x.Items.Contains(ushort.Parse(value)));

            if (set != null) {
                foreach (ushort item in set.Items) {
                    client.Character.Inventory.AddItem(item, 1);
                }
            }
            else
                client.Character.Reply("Set dosent exist");
        }


        [ChatCommand("gfx", ServerRoleEnum.Moderator)]
        public static void Gfx(string value, WorldClient client) {
            client.Character.SpellAnim(ushort.Parse(value));
        }


        [ChatCommand("item", ServerRoleEnum.Moderator)]
        public static void ItemCommand(string value, WorldClient client) {
            if (value == null || value.Split(' ').Length <= 0) {
                client.Character.ReplyError(".item [ItemId] [[Perfect]] [[Quantity]] [[Target Name]]");

                return;
            }

            string[] args = value.Split(' ');
            uint number = 1;
            Character target = null;
            ushort gid = ushort.Parse(args[0]);
            bool perfect = args.Length > 1;

            if (args.Length > 2) {
                number = UInt32.Parse(args[2]);
            }

            if (args.Length > 3) {
                target = WorldServer.Instance.GetOnlineClient(args[3]).Character;
            }

            if (target != null) {
                if (target.Inventory.AddItem(gid, number) != null) {
                    target.OnItemGained(gid, number);
                }
                else
                    client.Character.Reply("L'item n'éxiste pas...");

                return;
            }

            if (client.Character.Inventory.AddItem(gid, number, perfect) != null)
                client.Character.OnItemGained(gid, number);
            else
                client.Character.Reply("L'item n'éxiste pas...");
        }


        [ChatCommand("weapon", ServerRoleEnum.Moderator)]
        public static void WeaponCommand(string value, WorldClient client) {
            if (value == null || value.Split(' ').Length <= 0) {
                client.Character.ReplyError(".item [WeaponId] [[Perfect]] [[Quantity]] [[Target Name]]");

                return;
            }

            string[] args = value.Split(' ');
            uint number = 1;
            Character target = null;
            ushort gid = ushort.Parse(args[0]);
            bool perfect = args.Length > 1;

            if (args.Length > 2) {
                number = UInt32.Parse(args[2]);
            }

            if (args.Length > 3) {
                target = WorldServer.Instance.GetOnlineClient(args[3]).Character;
            }

            if (target != null) {
                if (target.Inventory.AddItem(gid, number) != null) {
                    target.OnItemGained(gid, number);
                }
                else
                    client.Character.Reply("L'item n'éxiste pas...");

                return;
            }

            if (client.Character.Inventory.AddItem(gid, number, perfect) != null)
                client.Character.OnItemGained(gid, number);
            else
                client.Character.Reply("L'item n'éxiste pas...");
        }


        [ChatCommand("help", ServerRoleEnum.Player)]
        public static void CommandsHelp(string value, WorldClient client) {
            client.Character.Reply("Chat Commands :");
            foreach (var item in CommandsHandler.Commands) {
                if (client.Account.Role >= item.Key.MinimumRoleRequired)
                    client.Character.Reply("-" + item.Key.Value);
            }
        }


        [ChatCommand("level", ServerRoleEnum.Administrator)]
        public static void LevelCommand(string value, WorldClient client) {
            if (value == null) {
                client.Character.ReplyError(".level [level] [target]");

                return;
            }

            if (value.Split(' ').Length < 2)
                client.Character.SetLevel(ushort.Parse(value));
            else {
                Character target = WorldServer.Instance.GetOnlineClient(value.Split(' ')[1]).Character;
                target?.SetLevel(ushort.Parse(value.Split(' ')[0]));
            }
        }


        [ChatCommand("joy", ServerRoleEnum.Administrator)]
        public static void JoyCommand(string value, WorldClient client) {
            List<ushort> spells = new List<ushort>();

            var items = ItemRecord.GetItems(ItemTypeEnum.FÉE_D_ARTIFICE);

            foreach (ItemRecord item in items) {
                SpellRecord spell = SpellRecord.Spells.Find(x => x.Name == item.Name);

                if (spell != null)
                    spells.Add(spell.Id);
            }

            foreach (ushort spell in spells) {
                Thread.Sleep(1000);
                client.Character.SpellAnim(spell);
            }
        }


        [ChatCommand("walk", ServerRoleEnum.Fondator)]
        public static void WalkCommand(string value, WorldClient client) {
            List<MapObstacle> obstacles = new List<MapObstacle>();
            for (ushort i = 0; i < 560; i++) {
                obstacles.Add(new MapObstacle(i, 1));
            }

            client.Send(new MapObstacleUpdateMessage(obstacles.ToArray()));
        }


        [ChatCommand("go", ServerRoleEnum.Animator)]
        public static void GoCommand(string value, WorldClient client) {
            client.Character.Teleport(int.Parse(value));
        }


        [ChatCommand("goto", ServerRoleEnum.Animator)]
        public static void GoToCommand(string value, WorldClient client) {
            if (value == null) {
                client.Character.ReplyError(".goto [TargetName]");

                return;
            }

            Character target = WorldServer.Instance.GetOnlineClient(value).Character;
            client.Character.Teleport(target.Map.Id, target.CellId);
        }


        [ChatCommand("restatall", ServerRoleEnum.Fondator)]
        public static void RestatCharacters(string value, WorldClient client) {
            foreach (CharacterRecord character in CharacterRecord.Characters) {
                WorldClient connected = WorldServer.Instance.GetOnlineClient(character.Id);

                if (connected != null) {
                    connected.Character.Restat();
                }
                else {
                    character.Restat(true);
                }
            }
            
            client.Character.Reply($"Restated {CharacterRecord.Characters} characters.");
        }


        [ChatCommand("restat", ServerRoleEnum.Animator)]
        public static void SelfRestat(string value, WorldClient client) {
            client.Character.Restat();
            client.Character.Reply("Restat successful.");
        }


        [ChatCommand("teleport", ServerRoleEnum.Animator)]
        public static void TeleportCommand(string value, WorldClient client) {
            string[] args = value.Split(' ');
            WorldClient target = null;
            WorldClient targetTo = client;
            if (args.Length > 0) {
                target = WorldServer.Instance.GetOnlineClient(args[0]);
            }

            if (args.Length > 1) {
                targetTo = WorldServer.Instance.GetOnlineClient(args[1]);
            }

            if (target != null && targetTo != null) {
                target.Character.Teleport(targetTo.Character.Map.Id, targetTo.Character.CellId);
            }
        }


        [ChatCommand("walkable", ServerRoleEnum.Fondator)]
        public static void WalkableCommand(string value, WorldClient client) {
            client.Send(new DebugClearHighlightCellsMessage());
            client.Send(new DebugHighlightCellsMessage(Color.BurlyWood.ToArgb(),
                                                       client.Character.Map.WalkableCells.ToArray()));
        }


        [ChatCommand("nospawn", ServerRoleEnum.Fondator)]
        public static void NoSpawnCommand(string value, WorldClient client) {
            // Already no spawn, re enable spawn
            if (MapNoSpawnRecord.MapsNoSpawns.Exists(r => r.MapId == client.Character.Map.Id)) {
                MapNoSpawnRecord record = MapNoSpawnRecord.MapsNoSpawns.Find(r => r.MapId == client.Character.Map.Id);
                record.RemoveInstantElement();

                client.Character.Map.Instance.Reload();
                client.Character.Reply("Monster spawn has be re-enabled on this map.");
            }
            // Otherwise, disable spawn
            else {
                new MapNoSpawnRecord(client.Character.Map.Id).AddInstantElement();
                foreach (MonsterGroup entity in client.Character.Map.Instance.GetEntities<MonsterGroup>()) {
                    client.Character.Map.Instance.RemoveEntity(entity);
                }

                client.Character.Reply("Monster spawn has be disabled on this map.");
            }
        }


        [ChatCommand("relative", ServerRoleEnum.Moderator)]
        public static void RelativeMapCommand(string value, WorldClient client) {
            var maps = MapRecord.Maps.FindAll(x => x.Position.Point == client.Character.Map.Position.Point);
            int index = maps.IndexOf(client.Character.Map);

            if (maps.Count > index + 1)
                client.Character.Teleport(maps[index + 1].Id);
            else
                client.Character.Teleport(maps[0].Id);
        }


        [ChatCommand("sun", ServerRoleEnum.Fondator)]
        public static void SunCommand(string value, WorldClient client) {
            var split = value.Split(null);

            int elementId = int.Parse(split[0]);

            int mapId = int.Parse(split[1]);

            short cellId = short.Parse(split[2]);

            InteractiveElementRecord element = InteractiveElementRecord.GetElement(elementId, client.Character.Map.Id);

            element.ChangeType(0);

            element.AddSmartSkill("Teleport", mapId.ToString(), cellId.ToString());

            client.Character.Map.Instance.Reload();

            client.Character.Reply("Soleil Ajouté");

            // return;

            // 2nd  mode
            /*       var split = value.Split(null); // split[0] = EleId, split[1] = destMapId, split[2] = destCellId

                   var element = InteractiveElementRecord.GetElement(int.Parse(split[0]), client.Character.Map.Id);

                   element.ChangeType(0);

                   element.AddSmartSkill("Teleport", split[1], split[2]);

                   client.Character.Map.Instance.ReloadInteractives();
                   client.Character.Reply("Soleil Ajouté"); */
        }


        [ChatCommand("mass", ServerRoleEnum.Fondator)]
        public static void MassCommand(string value, WorldClient client) {
            foreach (WorldClient target in WorldServer.Instance.GetOnlineClients()) {
                if (target.Character.Map != null && !target.Character.ChangeMap && target.Character.Map.Id != client.Character.Map.Id)
                    target.Character.Teleport(client.Character.Map.Id);
            }
        }


        [ChatCommand("gvg", ServerRoleEnum.Fondator)]
        public static void GvGCommand(string value, WorldClient client) {
            GuildArenaProvider.Register(client.Character);
        }


        [ChatCommand("sortGvG", ServerRoleEnum.Fondator)]
        public static void SortGvGCommand(string value, WorldClient client) {
            GuildArenaProvider.Sort();
            GuildArenaProvider.Fight();
        }


        [ChatCommand("clone", ServerRoleEnum.Fondator)]
        public static void CloneCommand(string value, WorldClient client) {
            CharacterItemRecord weapon = client.Character.Inventory.GetWeapon();

            weapon.Mage(EffectsEnum.Effect_DamageFire, 85);

            client.Character.Inventory.OnItemModified(weapon);
        }


        [ChatCommand("avert", ServerRoleEnum.Moderator)]
        public static void AvertCommad(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                target.Character.OpenPopup(10, client.Character.Name, "Veuillez modérer votre language.");
            }
        }


        [ChatCommand("endfight", ServerRoleEnum.Fondator)]
        public static void EndFightCommand(string value, WorldClient client) {
            var split = value.Split(' ');

            int map = int.Parse(split[0]);
            MapRecord record = MapRecord.GetMap(map);

            ushort randomWalkableCell = split.Length > 1 ? ushort.Parse(split[1]) : record.RandomWalkableCell();
            
            EndFightActionRecord endFight = new EndFightActionRecord(EndFightActionRecord.EndFightActions.DynamicPop(x => x.Id),
                                                                     client.Character.Map.Id,
                                                                     map,
                                                                     randomWalkableCell);

            endFight.AddInstantElement();

            client.Character.Reply("EndFightAction has been added");
        }


        [ChatCommand("randomMonsters", ServerRoleEnum.Fondator)]
        public static void RandomMontersCommand(string value, WorldClient client) {
            var list = MonsterRecord.Monsters.FindAll(x => x.Grades[0].Level >= client.Character.Level)
                                    .Random(int.Parse(value));

            MonsterSpawnManager.Instance.AddFixedMonsterGroup(client.Character.Map.Instance, list.ToArray(), false);

            client.Character.Reply("Monsters added!");
        }


        [ChatCommand("mination", ServerRoleEnum.Fondator)]
        public static void MinationCommand(string value, WorldClient client) {
            CharacterItemRecord mination = MinationLoot.CreateMinationItem(ItemRecord.GetItem(MinationLoot.MinationBossItemId),
                                                                           1,
                                                                           client.Character.Id,
                                                                           MonsterRecord.GetMonster(ushort.Parse(value)),
                                                                           1);
            client.Character.Inventory.AddItem(mination);
        }


        [ChatCommand("addForbidden", ServerRoleEnum.Fondator)]
        public static void AddForbiddenMonsterToMinationCommand(string value, WorldClient client) {
            MinationLoot.AddForbiddenMonster(ushort.Parse(value));

            foreach (WorldClient target in WorldServer.Instance.GetClients()) {
                target.Character.Inventory.RemoveForbiddenItems();
            }

            client.Character.Reply("Done.");
        }

        /// <summary>
        /// 133 = parasol
        /// </summary>
        /// <param name="value"></param>
        /// <param name="client"></param>
        [ChatCommand("mapemote", ServerRoleEnum.Fondator)]
        public static void MapEmoteCommand(string value, WorldClient client) {
            var split = value.Split(null);
            foreach (Character character in client.Character.Map.Instance.GetEntities<Character>()) {
                character.PlayEmote(byte.Parse(split[0]));
            }

            if (split.Length == 2) {
                foreach (Npc npc in client.Character.Map.Instance.GetEntities<Npc>()) {
                    npc.Say(split[1].Replace('_', ' '));
                }
            }
        }


        [ChatCommand("test", ServerRoleEnum.Fondator)]
        public static void TestCommand(string value, WorldClient client) {
            client.SendRaw(RawDataManager.GetRawData("gvgpanel"));
            client.Character.Reply("Done");

            //  client.Character.SpellAnim(7356);
            // return;
            // client.Character.Record.Stats.LifePoints = 99999;
            // client.Character.Record.Stats.ActionPoints.Base += 99;
            // client.Character.Record.Stats.MaxLifePoints = 99999;
            // client.Character.RefreshStats();
            //
            // return;
            // int id = DelayedActionRecord.DelayedActions.DynamicPop(x => x.Id);
            // DelayedActionRecord record =
            //     new DelayedActionRecord(id, "CharacterMonster", 50, value, client.Character.Map.Id.ToString());
            // record.AddElement();
            // DelayedAction action = new DelayedAction(record);
            // DelayedActionManager.AddAction(action);
            // action.Execute();
        }


        [ChatCommand("zaap", ServerRoleEnum.Fondator)]
        public static void Zaap(string value, WorldClient client) {
            InteractiveElementRecord element = InteractiveElementRecord.GetElement(int.Parse(value), client.Character.Map.Id);
            element.ElementType = 16;
            InteractiveSkillRecord skill = new InteractiveSkillRecord(InteractiveSkillRecord.InteractiveSkills.DynamicPop(x => x.UID),
                                                                      "Zaap",
                                                                      "Global",
                                                                      "",
                                                                      element.UId,
                                                                      114);

            element.UpdateInstantElement();
            skill.AddInstantElement();
            client.Character.Map.Instance.Reload();
        }


        [ChatCommand("placement", ServerRoleEnum.Moderator)]
        public static void PlacementCommand(string value, WorldClient client) {
            client.Character.Map.Instance.ShowPlacement(client.Character);
        }


        [ChatCommand("addPlacement", ServerRoleEnum.Moderator)]
        public static void AddPlacementCommand(string value, WorldClient client) {
            client.Character.Map.Instance.AddPlacement((TeamColorEnum) int.Parse(value),
                                                       (short) client.Character.CellId);
            client.Character.Map.Instance.ShowPlacement(client.Character);
        }


        [ChatCommand("look", ServerRoleEnum.Moderator)]
        public static void LookCommand(string value, WorldClient client) {
            value = value.Replace("&#123;", "{").Replace("&#125;", "}");
            Console.WriteLine($"Changing look of player {client.Character.Name} ({client.Account.Nickname}): from {client.Character.Look} to {value}.");
            client.Character.Reply($"Look before update: {client.Character.Look}");
            
            client.Character.Look = ContextActorLook.Parse(value);
            client.Character.RefreshActorOnMap();
        }


        [ChatCommand("getLook", ServerRoleEnum.Moderator)]
        public static void GetLookCommand(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                client.Character.Reply("Target look is: " + ContextActorLook.ConvertToString(target.Character.Look));
            }
            else {
                client.Character.Reply("Your look is: " + ContextActorLook.ConvertToString(client.Character.Look));
            }
        }


        [ChatCommand("updateCharacters", ServerRoleEnum.Fondator)]
        public static void ReloadCharacters(string value, WorldClient client) {
            foreach (CharacterRecord character in CharacterRecord.Characters) {
                character.UpdateElement();
            }
        }


        [ChatCommand("unequip", ServerRoleEnum.Fondator)]
        public static void Unequip(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null && !target.Character.Fighting) {
                target.Character.Inventory.UnequipAll();
            }
        }


        [ChatCommand("start")]
        public static void StartCommand(string value, WorldClient client) {
            client.Character.Teleport(154010883, 383);
        }


        [ChatCommand("infos", ServerRoleEnum.Player)]
        public static void Infos(string value, WorldClient client) {
            client.Character.Reply("Clients Connecteds: " + WorldServer.Instance.ClientsCount);

            List<string> clients = WorldServer.Instance.GetOnlineClients().ConvertAll(x => x.Character.Name);

            client.Character.Reply(clients.ToCSV());

            if (client.Account.Role > ServerRoleEnum.Player) {
                client.Character.Reply("Connected Distincted :"
                                       + WorldServer.Instance.GetOnlineClients()
                                                    .ConvertAll(x => x.Ip)
                                                    .Distinct()
                                                    .Count());
            }
        }


        [ChatCommand("spell", ServerRoleEnum.Moderator)]
        public static void Spell(string value, WorldClient client) {
            string[] args = value.Split(' ');

            if (args.Length == 1) {
                client.Character.LearnSpell(ushort.Parse(args[0]));
            }
            else if (args.Length == 2) {
                WorldClient target = WorldServer.Instance.GetOnlineClient(args[1]);
                target.Character.LearnSpell(ushort.Parse(args[0]));
                client.Character.Reply("Done!");
            }
        }


        [ChatCommand("addhonor", ServerRoleEnum.Moderator)]
        public static void AddHonorCommand(string value, WorldClient client) {
            string[] args = value.Split(' ');
            if (args.Length == 0) {
                client.Character.ReplyError(".addhonor [Honor] [[Target]]");

                return;
            }

            if (args.Length == 1) {
                client.Character.AddHonor(ushort.Parse(args[0]));
            }

            if (args.Length == 2) {
                WorldServer.Instance.GetOnlineClient(args[1]).Character.AddHonor(ushort.Parse(args[0]));
            }
        }


        [ChatCommand("mutemap", ServerRoleEnum.Moderator)]
        public static void MuteMapCommand(string value, WorldClient client) {
            client.Character.Map.Instance.ToggleMute();
            client.Character.Reply("Effectué!");
        }


        [ChatCommand("ban", ServerRoleEnum.Moderator)]
        public static void Ban(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                if (target.Ban())
                    client.Character.Reply("Le compte " + target.Account.Nickname + " a été banni.");
            }
        }


        [ChatCommand("mute", ServerRoleEnum.Animator)]
        public static void Mute(string value, WorldClient client) {
            var splitted = value.Split(null);
            int minutes = int.Parse(splitted[0]);
            WorldClient target = WorldServer.Instance.GetOnlineClient(splitted[1]);

            if (target != null) {
                if (target.Character.Mute(minutes * 60)) {
                    client.Character.Reply("Le joueur a été mute " + minutes + " minutes");
                    target.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                                                     17,
                                                     client.Character.Name,
                                                     minutes);
                    target.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 107);
                }
                else
                    client.Character.Reply("Le joueur est déja mute.");
            }
            else {
                client.Character.Reply("Le joueur n'a pas été trouvé");
            }
        }


        [ChatCommand("dev", ServerRoleEnum.Fondator)]
        public static void Dev(string value, WorldClient client) {
            foreach (CharacterSpell spell in client.Character.Record.Spells) {
                foreach (SpellLevelRecord level in spell.Template.Levels) {
                    EffectInstance eff1 = new EffectInstance() {
                        Delay = 0,
                        DiceMin = 6,
                        EffectId = (ushort) EffectsEnum.Effect_SubMP_1080,
                        TargetMask = "A",
                        RawZone = level.Effects[0].RawZone
                    };
                    EffectInstance eff2 = new EffectInstance() {
                        Delay = 0,
                        DiceMin = 30,
                        EffectId = (ushort) EffectsEnum.Effect_PushBack,
                        TargetMask = "A",
                        RawZone = level.Effects[0].RawZone
                    };
                    level.Effects.Add(eff1);
                    level.Effects.Add(eff2);
                    level.CriticalEffects.Add(eff1);
                    level.CriticalEffects.Add(eff2);
                }
            }

            client.Character.Reply("done :p");
        }


        [ChatCommand("calc", ServerRoleEnum.Fondator)]
        public static void Raw(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                target.SendRaw(RawDataManager.GetRawData("overcalc"));
                client.Character.Reply("Done");
            }
        }


        [ChatCommand("hiber", ServerRoleEnum.Fondator)]
        public static void Hiber(string value, WorldClient client) {
            WorldClient target = WorldServer.Instance.GetOnlineClient(value);

            if (target != null) {
                target.SendRaw(RawDataManager.GetRawData("hibernate"));
                client.Character.Reply("Done");
            }
        }


        [ChatCommand("lion", ServerRoleEnum.Animator)]
        public static void Lion(string value, WorldClient client) {
            LookCommand("{1003}", client);
        }


        [ChatCommand("event")]
        public static void Event(string value, WorldClient client) {
            client.Character.Teleport(148636161, 398);
        }


        [ChatCommand("mapids", ServerRoleEnum.Fondator)]
        public static void MapIds(string value, WorldClient client) {
            var allMapsAtPlayerCoordinates = MapPositionRecord.MapPositions.FindAll(rec => rec.X == client.Character.Map.X && rec.Y == client.Character.Map.Y);

            string outdoorMapsCsv = String.Join(", ", allMapsAtPlayerCoordinates.FindAll(map => map.Outdoor).ConvertAll(map => map.Id));
            string indoorMapsCsv = String.Join(", ", allMapsAtPlayerCoordinates.FindAll(map => !map.Outdoor).ConvertAll(map => map.Id));

            client.Character.Reply($"Outdoor maps: {outdoorMapsCsv}");
            client.Character.Reply($"Indoor maps: {indoorMapsCsv}");
        }


        // [ChatCommand("addpaddock", ServerRoleEnum.Fondator)]
        // public static void AddPaddock(string value, WorldClient client) {
        //     InteractiveElementsUtils.CreateInteractiveElement(new NewElementData(client.Character.Map.Id, int.Parse(value), 120, "Paddock", 114), client);
        // }
        //
        // // value: <element id> <mapid> <cellid>
        // // Ex: 54315631 123456145 125
        //
        // [ChatCommand("adddoor", ServerRoleEnum.Fondator)]
        // public static void AddDoor(string value, WorldClient client) {
        //     var split = value.Split(' ');
        //
        //     int elementId = int.Parse(split[0]);
        //     string value1 = split[1];
        //     string value2 = split[2];
        //
        //     InteractiveElementsUtils.CreateInteractiveElement(new NewElementData(client.Character.Map.Id, elementId, 70, "Teleport", 84, value1, value2), client);
        // }


        // [ChatCommand("dropupdate", ServerRoleEnum.Fondator)]
        // public static void UpdateDrop(string value, WorldClient client) {
        //     var split = value.Split(' ');
        //
        //     ushort itemId = ushort.Parse(split[0]);
        //     short dropRate = short.Parse(split[1]);
        //     ushort monsterId = ushort.Parse(split[2]);
        //
        //     ItemRecord item = ItemRecord.GetItem(itemId);
        //     MonsterRecord monsterRecord = MonsterRecord.GetMonster(monsterId);
        //     MonsterDrop monsterDrop = monsterRecord.Drops.Find(d => d.ItemId == itemId);
        //
        //     monsterDrop.PercentDropForGrade1 = dropRate;
        //     monsterDrop.PercentDropForGrade2 = (short) (dropRate + 2);
        //     monsterDrop.PercentDropForGrade3 = (short) (dropRate + 4);
        //     monsterDrop.PercentDropForGrade4 = (short) (dropRate + 6);
        //     monsterDrop.PercentDropForGrade5 = (short) (dropRate + 8);
        //
        //     monsterRecord.UpdateInstantElement();
        //
        //     client.Character.Reply($"Successfully removed drop '{item.Name}' ({item.Id}) from monster '{monsterRecord.Name}' {monsterId}");
        // }


        // [ChatCommand("droprm", ServerRoleEnum.Fondator)]
        // public static void RemoveDrop(string value, WorldClient client) {
        //     var split = value.Split(' ');
        //
        //     ushort itemId = ushort.Parse(split[0]);
        //     ushort monsterId = ushort.Parse(split[1]);
        //
        //     ItemRecord item = ItemRecord.GetItem(itemId);
        //     MonsterRecord monsterRecord = MonsterRecord.GetMonster(monsterId);
        //     monsterRecord.Drops.RemoveAll(d => d.ItemId == itemId);
        //
        //     monsterRecord.UpdateInstantElement();
        //
        //     client.Character.Reply($"Successfully removed drop '{item.Name}' ({item.Id}) from monster '{monsterRecord.Name}' {monsterId}");
        // }

        // [ChatCommand("dropadd", ServerRoleEnum.Fondator)]
        // public static void AddDrop(string value, WorldClient client) {
        //     var split = value.Split(' ');
        //
        //     ushort itemId = ushort.Parse(split[0]);
        //     short dropRate = short.Parse(split[1]);
        //
        //     ItemRecord item = ItemRecord.GetItem(itemId);
        //
        //     int nextDropId = MonsterRecord.GetNextDropId();
        //
        //     for (var i = 2; i < split.Length; i++) {
        //         ushort monsterId = ushort.Parse(split[i]);
        //
        //         MonsterRecord monster = MonsterRecord.GetMonster(monsterId);
        //
        //         MonsterDrop drop = new MonsterDrop() {
        //             ItemId = itemId,
        //             Count = 1,
        //             DropId = nextDropId,
        //             DropLimit = 1,
        //             HasCriteria = false,
        //             PercentDropForGrade1 = dropRate,
        //             PercentDropForGrade2 = (short) (dropRate + 2),
        //             PercentDropForGrade3 = (short) (dropRate + 4),
        //             PercentDropForGrade4 = (short) (dropRate + 6),
        //             PercentDropForGrade5 = (short) (dropRate + 8),
        //             ProspectingLock = 100
        //         };
        //
        //         monster.Drops.Add(drop);
        //
        //         // TODO: check if necessary.
        //         monster.UpdateInstantElement();
        //
        //         client.Character.Reply($"Added drop '{item.Name}' ({itemId}) to monster '{monster.Name}' ({monsterId}) with a drop rate of {dropRate} (DropId={nextDropId}).");
        //         nextDropId++;
        //     }
        // }

        [ChatCommand("el", ServerRoleEnum.Fondator)]
        public static void AddElementAbbr(string value, WorldClient client) => ElementsCmdBroker.Run(value, client);

        [ChatCommand("elements", ServerRoleEnum.Fondator)]
        public static void AddElement(string value, WorldClient client) => ElementsCmdBroker.Run(value, client);


        [ChatCommand("link", ServerRoleEnum.Fondator)]
        public static void Link(string value, WorldClient client) => LinksCmdBroker.Run(value, client);

        [ChatCommand("entries", ServerRoleEnum.Fondator)]
        public static void Entries(string value, WorldClient client) => EntriesCmdBroker.Run(value, client);
        
        [ChatCommand("exits", ServerRoleEnum.Fondator)]
        public static void Exits(string value, WorldClient client) => ExitsCmdBroker.Run(value, client);

        [ChatCommand("mine", ServerRoleEnum.Fondator)]
        public static void Mine(string value, WorldClient client) => MineCmdBroker.Run(value, client);

        [ChatCommand("drops", ServerRoleEnum.Fondator)]
        public static void Drops(string value, WorldClient client) => DropsCmdBroker.Run(value, client);

        [ChatCommand("npc", ServerRoleEnum.Fondator)]
        public static void Npcs(string value, WorldClient client) => NpcCmdBroker.Run(value, client);
    }
}