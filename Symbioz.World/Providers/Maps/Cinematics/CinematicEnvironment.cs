using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Entities.Look;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Providers.Delayed;
using Symbioz.World.Providers.Fights;
using Symbioz.World.Records.Almanach;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Portals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Symbioz.Protocol.Selfmade.Enums;

namespace Symbioz.World.Providers.Maps.Cinematics {
    /// <summary>
    /// This is only experimental, this is not verified and performances can be slow are just bad. 
    /// </summary>
    public class CinematicEnvironment {
        private Character Character { get; set; }
        private int Delay { get; set; }
        private CinematicScript Script { get; set; }
        private Npc Npc { get; set; }

        public CinematicEnvironment(Character character, CinematicScript script) {
            this.Character = character;
            this.Script = script;
            this.Npc = character.Map.Instance.GetNpc((int) script.NpcId);
        }

        public async void canInteract(bool ableTo) {
            var action = new Action(() => { this.Character.CanInteract = ableTo; });
            await this.Execute(action);
        }

        public async void say(string message) {
            var action = new Action(() => { this.Character.Say(this.Character, message); });
            await this.Execute(action);
        }

        public async void say(int entityId, string message) {
            var action = new Action(() => { this.Character.Map.Instance.GetEntity(entityId).Say(this.Character, message); });
            await this.Execute(action);
        }

        public async void sayNpc(string message) {
            var action = new Action(() => { this.Npc.Say(this.Character, message); });
            await this.Execute(action);
        }

        public async void spellAnim(short spellId) {
            var action = new Action(() => { this.Character.SpellAnim(this.Character, (ushort) spellId); });
            await this.Execute(action);
        }

        public async void notification(string message) {
            var action = new Action(() => { this.Character.Notification(message); });
            await this.Execute(action);
        }

        public async void teleport(int mapId) {
            var action = new Action(() => { this.Character.Teleport(mapId, null, false, true); });
            await this.Execute(action);
        }

        public async void dayfight() {
            var action = new Action(() => { PvMInstancesManager.Instance.Dayfight(this.Character); });
            await this.Execute(action);
        }

        public async void teleportSameMap(short cellId) {
            var action = new Action(() => { this.Character.Teleport(this.Character.Map.Id, (ushort) cellId, false, true); });
            await this.Execute(action);
        }

        public async void noMove() {
            var action = new Action(() => { this.Character.NoMove(); });
            await this.Execute(action);
        }

        public async void orientation(int orientation) {
            var action = new Action(() => { this.Character.SetDirection((DirectionsEnum) orientation); });
            await this.Execute(action);
        }

        public async void video(short id) {
            var action = new Action(() => { this.Character.PlayCinematic((ushort) id); });
            await this.Execute(action);
        }

        public async void smiley(short id) {
            var action = new Action(() => { this.Character.DisplaySmiley((ushort) id); });
            await this.Execute(action);
        }

        public async void smiley(long entityId, short id) {
            var action = new Action(() => { this.Character.Map.Instance.GetEntity(entityId).DisplaySmiley(this.Character, (ushort) id); });
            await this.Execute(action);
        }

        public async void addItem(short id, int quantity) {
            var action = new Action(() => {
                this.Character.Inventory.AddItem((ushort) id, (uint) quantity);
                this.Character.OnItemGained((ushort) id, (uint) quantity);
            });
            await this.Execute(action);
        }

        public async void removeFirstItemByType(int typeId) {
            var action = new Action(() => {
                ItemTypeEnum type = (ItemTypeEnum) typeId;
                CharacterItemRecord item = this.Character.Inventory.GetFirstItem(type);

                if (item != null) {
                    this.Character.Inventory.RemoveItem(item, 1);
                    this.Character.OnItemLost(item.GId, 1);
                }
            });
            await this.Execute(action);
        }

        public async void removeItem(short id, int quantity) {
            var action = new Action(() => {
                CharacterItemRecord item = this.Character.Inventory.GetFirstItem((ushort) id, (uint) quantity);

                if (item != null) {
                    this.Character.Inventory.RemoveItem(item, (uint) quantity);
                    this.Character.OnItemLost(item.GId, (uint) quantity);
                }
            });
            await this.Execute(action);
        }

        public async void smileyNpc(short id) {
            var action = new Action(() => { this.Npc.DisplaySmiley(this.Character, (ushort) id); });
            await this.Execute(action);
        }

        public async void addOrnament(short id) {
            var action = new Action(() => { this.Character.LearnOrnament((ushort) id, true); });
            await this.Execute(action);
        }

        public async void addTitle(short id) {
            var action = new Action(() => { this.Character.LearnTitle((ushort) id); });
            await this.Execute(action);
        }

        public async void reach(short id) {
            var action = new Action(() => { this.Character.ReachObjective(id); });
            await this.Execute(action);
        }

        public async void setAlign(int align) {
            var action = new Action(() => { this.Character.SetSide((AlignmentSideEnum) align); });
            await this.Execute(action);
        }

        public async void restat() {
            var action = new Action(() => { this.Character.Restat(true); });
            await this.Execute(action);
        }

        public async void reply(string str) {
            var action = new Action(() => { this.Character.Reply(str); });
            await this.Execute(action);
        }

        public async void createGuild() {
            var action = new Action(() => { this.Character.OpenGuildCreationPanel(); });
            await this.Execute(action);
        }

        public async void SendRawData(string name) {
            var action = new Action(() => { this.Character.Client.SendRaw(RawDataManager.GetRawData(name)); });
            await this.Execute(action);
        }

        public void wait(int ms) {
            this.Delay += ms;
        }

        public async void fight(short monsterId) {
            var action = new Action(() => {
                var newFight = FightProvider.Instance.CreateFreeFightInstancePvM(new MonsterRecord[] {MonsterRecord.GetMonster((ushort) monsterId)}, this.Character.Map);

                FightTeam characterTeam = newFight.GetTeam(TeamTypeEnum.TEAM_TYPE_PLAYER);
                FightTeam monsterTeam = newFight.GetTeam(TeamTypeEnum.TEAM_TYPE_MONSTER);

                characterTeam.AddFighter(this.Character.CreateFighter(characterTeam));

                foreach (var monster in newFight.Group.CreateFighters(monsterTeam)) {
                    monsterTeam.AddFighter(monster);
                }
            });
            await this.Execute(action);
        }

        public bool hasDoneObjective(short id) {
            return this.Character.HasReachObjective(id);
        }

        public string getPortalPositionString(string name) {
            var portal = PortalRecord.GetPortal(name);
            var action = DelayedActionManager.GetAction(DelayedActionEnum.Portal, portal.Id.ToString());

            var nextPop = action.Timer.Interval.ConvertToMinutes();

            if (action == null || action.Value == null)
                return "?";

            var map = (MapRecord) action.Value;

            return string.Format("<b>[{0},{1}]</b> ({2}) il y est encore pour " + Math.Ceiling(nextPop) + " minutes", map.Position.X, map.Position.Y, map.SubArea.Name);
        }

        public bool canAlmanach() {
            var almanach = AlmanachRecord.GetAlmanachOfTheDay();
            if (almanach != null) {
                return this.Character.CanAlmanach(almanach);
            }
            else {
                return true;
            }
        }

        public bool honorAlmanach() {
            var almanach = AlmanachRecord.GetAlmanachOfTheDay();

            if (almanach != null) {
                return this.Character.DoAlmanach(almanach);
            }
            else {
                return false;
            }
        }

        public string getAlmanachRequestString() {
            var almanach = AlmanachRecord.GetAlmanachOfTheDay();

            ItemRecord item = ItemRecord.GetItem(almanach.ItemGId);

            if (almanach == null || item == null) {
                return "Aucune récompense n'est prévue aujourd'hui, repasse demain.";
            }
            else {
                return string.Format("Rapporte moi {0}x <b>[{1}]</b> et tu auras honoré les merydes de t'avoir donner la vie.", almanach.Quantity, item.Name);
            }
        }

        public string getZoneExperienceRate() {
            return this.Character.Map.SubArea.ExperienceRate.ToString();
        }

        public string getPlayerName() {
            return this.Character.Name;
        }

        public string getGuildName() {
            return this.Character.HasGuild ? this.Character.Guild.Record.Name : "?";
        }

        public async Task Execute(Action action) {
            await Task.Delay(this.Delay);
            this.Delay = 0;
            try {
                action();
            }
            catch (Exception ex) {
                Logger.Write<CinematicEnvironment>(ex.ToString(), ConsoleColor.Red);
            }
        }
    }
}