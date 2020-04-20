using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Fights;

namespace Symbioz.World.Providers.Brain.Behaviors.Salbatroces {
    [Behavior("Malamibe")]
    public class Malamibe : Behavior {
        public const short HP_PER_PLAYER = 2000;

        public const int MAX_TURN_COUNT_ALIVE = 3;

        private int TurnCount { get; set; }

        public void OnSummoned() {
            foreach (var fighter in this.Fighter.OposedTeam().GetFighters()) {
                fighter.AfterDeadEvt += this.Fighter_AfterDeadEvt;
                this.Fighter.AddLife(HP_PER_PLAYER, false);
            }

            foreach (var ally in this.Fighter.Team.GetFighters()) {
                if (ally != this.Fighter) // Stack Overflow
                    ally.OnAttemptToInflictEvt += this.OnAllyInflicted;
            }

            this.Fighter.BeforeTakeDamagesEvt += this.Fighter_BeforeTakeDamagesEvt;
            this.Fighter.ShowFighter();
        }

        private void Fighter_BeforeTakeDamagesEvt(Fighter arg1, Models.Fights.Damages.Damage arg2) {
            if (arg2.Target == this.Fighter) {
                arg2.Delta /= 2;
            }
        }

        private void OnAllyInflicted(Fighter arg1, Models.Fights.Damages.Damage arg2) {
            this.Fighter.InflictDamages(arg2);
        }

        private void Fighter_AfterDeadEvt(Fighter obj, bool recursiveCall) {
            this.Fighter.SubLife(HP_PER_PLAYER);
        }

        public Malamibe(BrainFighter fighter) : base(fighter) {
            this.Fighter.OnTurnStartEvt += this.Fighter_OnTurnStartEvt;
        }


        private void Fighter_OnTurnStartEvt(Fighter obj) {
            this.TurnCount++;

            if (this.TurnCount == MAX_TURN_COUNT_ALIVE) {
                this.Fighter.OposedTeam().KillTeam();
            }
        }
    }
}