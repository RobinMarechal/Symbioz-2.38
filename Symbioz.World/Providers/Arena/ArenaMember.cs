using SSync.Messages;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.World.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Arena {
    public class ArenaMember {
        public Character Character { get; private set; }

        public ArenaGroup Group {
            get { return this.Team.Group; }
        }

        public ArenaMemberCollection Team { get; private set; }
        public PvpArenaStepEnum Step { get; private set; }
        public bool Accepted { get; private set; }

        public ArenaMember(Character character, ArenaMemberCollection team) {
            this.Character = character;
            this.Team = team;
            this.Accepted = false;
        }

        private void UpdateRegistrationStatus(bool registred) {
            this.Send(new GameRolePlayArenaRegistrationStatusMessage(registred, (sbyte) this.Step, (int) this.Group.Type));
        }

        public void UpdateStep(bool registred, PvpArenaStepEnum step) {
            this.Step = step;
            this.UpdateRegistrationStatus(registred);
        }

        public void Send(Message message) {
            this.Character.Client.Send(message);
        }

        public void Request() {
            this.Character.Client.Send(new GameRolePlayArenaFightPropositionMessage(0, Array.ConvertAll(this.Team.GetMemberIds(), x => (double) x), this.Group.RequestDuration));
        }

        public void RejoinMap() {
            this.Character.RejoinMap(FightTypeEnum.FIGHT_TYPE_PVP_ARENA, false, true);
        }

        private void UpdateFighterStatus() {
            this.Group.Send(new GameRolePlayArenaFighterStatusMessage(0, (int) this.Character.Id, this.Accepted));
        }

        public void Anwser(bool accept) {
            this.Accepted = accept;
            this.UpdateFighterStatus();

            if (this.Accepted) {
                if (this.Group.Accepted && this.Group.Ready) {
                    this.Group.StartFighting();
                }
            }
            else {
                this.Character.UnregisterArena();
                this.Team.ForEach(x => x.UpdateStep(true, PvpArenaStepEnum.ARENA_STEP_REGISTRED));
                this.Team.ForEach(x => x.Accepted = false);
            }
        }
    }
}