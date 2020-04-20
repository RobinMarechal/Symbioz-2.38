using Symbioz.World.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz.Protocol.Enums;
using System.Threading.Tasks;
using Symbioz.Protocol.Messages;
using SSync.Messages;
using Symbioz.World.Models.Fights;
using Symbioz.Core;
using Symbioz.World.Providers.Fights;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Providers.Arena {
    public class ArenaGroup {
        public const ushort LEVEL_SHIFT = 10;

        public CharacterInventoryPositionEnum[] NoPositions = new CharacterInventoryPositionEnum[] {
            CharacterInventoryPositionEnum.INVENTORY_POSITION_COMPANION,
            CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD,
        };


        public virtual PvpArenaTypeEnum Type {
            get { return PvpArenaTypeEnum.ARENA_TYPE_1VS1; }
        }

        public virtual ushort RequestDuration {
            get { return 0; }
        }

        /// <summary>
        /// Le joueur a t-il le niveau pour rejoindre ce groupe
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool CanChallenge(Character character) {
            if (this.Empty) // Si le groupe est vide, ... il a forcement le niveau
            {
                return true;
            }

            double mediumLevel = 1;
            double med = Extensions.Med(Array.ConvertAll(this.BlueTeam.GetMembers(), x => x.Character.Level)); // niveau moyen de l'équipe bleue
            double med2 = Extensions.Med(Array.ConvertAll(this.RedTeam.GetMembers(), x => x.Character.Level)); // niveau moyen de l'équipe rouge

            if (med2 == 0) // si il n'y a aucun membre dans l'équipe rouge
            {
                mediumLevel = med;
            }
            else if (med == 0) // si il n'y a aucun membre dans l'équipe bleue
            {
                mediumLevel = med2;
            }
            else {
                mediumLevel = Extensions.Med(new double[] {med, med2}); // on calcule la moyenne de niveau entre les deux groupes
            }

            return character.Level > mediumLevel - LEVEL_SHIFT && character.Level < mediumLevel + LEVEL_SHIFT; // mediumLevel - shift < x < mediumLevel + shift
        }

        public bool Ready {
            get { return this.BlueTeam.IsFull && this.RedTeam.IsFull; }
        }

        public bool Accepted {
            get { return this.BlueTeam.Accepted && this.RedTeam.Accepted; }
        }

        private bool IsTeamFull(List<Character> team) {
            return team.Count == (int) this.Type;
        }

        public ArenaGroup() {
            this.BlueTeam = new ArenaMemberCollection(this);
            this.RedTeam = new ArenaMemberCollection(this);
        }

        protected ArenaMemberCollection BlueTeam { get; private set; }
        protected ArenaMemberCollection RedTeam { get; private set; }

        /// <summary>
        /// Todo
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected ArenaMemberCollection SelectTeam(Character character) {
            if (!this.BlueTeam.IsFull) {
                return this.BlueTeam;
            }
            else if (!this.RedTeam.IsFull) {
                return this.RedTeam;
            }
            else {
                throw new Exception("Both teams are full, cannot add character");
            }
        }


        public ArenaMember AddCharacter(Character character) {
            return this.SelectTeam(character).AddMember(this, character);
        }

        public void Send(Message message) {
            this.ForEach(x => x.Send(message));
        }

        public void ForEach(Action<ArenaMember> action) {
            foreach (var member in this.BlueTeam.GetMembers()) {
                action(member);
            }

            foreach (var member in this.RedTeam.GetMembers()) {
                action(member);
            }
        }

        public void Request() {
            this.ForEach(x => x.Request());
            this.ForEach(x => x.UpdateStep(false, PvpArenaStepEnum.ARENA_STEP_WAITING_FIGHT));
        }

        public bool Empty {
            get { return this.BlueTeam.Empty && this.RedTeam.Empty; }
        }

        public ArenaMember[] GetAllMembers() {
            return this.BlueTeam.GetMembers().Concat(this.RedTeam.GetMembers()).ToArray();
        }

        public void CheckIntegrity(ArenaMember member) {
            foreach (var noPos in this.NoPositions) {
                if (member.Character.Inventory.Unequip(noPos)) {
                    member.Character.OnItemUnequipedArena();
                }
            }
        }

        public void StartFighting() {
            this.ForEach(x => x.Character.PreviousRoleplayMapId = x.Character.Record.MapId);
            this.ForEach(x => x.UpdateStep(false, PvpArenaStepEnum.ARENA_STEP_STARTING_FIGHT));

            MapRecord map = ArenaMapRecord.GetArenaMap();

            foreach (var member in this.BlueTeam.GetMembers()) {
                member.Character.Teleport(map.Id, null, true);
                this.CheckIntegrity(member);
            }

            foreach (var member in this.RedTeam.GetMembers()) {
                member.Character.Teleport(map.Id, null, true);
                this.CheckIntegrity(member);
            }

            FightArena fight = FightProvider.Instance.CreateFightArena(map);

            foreach (var member in this.BlueTeam.GetMembers()) {
                fight.BlueTeam.AddFighter(member.Character.CreateFighter(fight.BlueTeam));
            }

            foreach (var member in this.RedTeam.GetMembers()) {
                fight.RedTeam.AddFighter(member.Character.CreateFighter(fight.RedTeam));
            }


            fight.StartPlacement();
            this.Dispose();
        }

        public bool ContainsIp(string ip) {
            return this.GetAllMembers().Any(x => x.Character.Client.Ip == ip);
        }

        public void Dispose() {
            ArenaProvider.Instance.ArenaGroups.Remove(this);
            this.ForEach(x => x.Character.UnregisterArena());
            this.ForEach(x => x.UpdateStep(false, PvpArenaStepEnum.ARENA_STEP_UNREGISTER));
            this.RedTeam = null;
            this.BlueTeam = null;
        }
    }
}