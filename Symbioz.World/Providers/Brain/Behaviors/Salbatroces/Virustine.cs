using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Behaviors.Salbatroces {
    /// <summary>
    /// Ne doit pas être poussé pendant 5 tour
    /// </summary>
    [Behavior("Virustine")]
    public class Virustine : SalbatroceBehavior {
        public const int TURN_COUNT = 5;

        public int TurnCount { get; private set; }

        private bool Invunlnerable {
            get { return this.Fighter.HasState(x => x.Invulnerable); }
        }

        public Virustine(BrainFighter fighter) : base(fighter) {
            fighter.Fight.FightStartEvt += this.Fight_FightStartEvt;
            fighter.AfterSlideEvt += this.Fighter_AfterSlideEvt;
            this.Fighter.OnTurnStartEvt += this.Fighter_OnTurnStartEvt;
            this.TurnCount = 0;
        }

        private void Fighter_AfterSlideEvt(Fighter target, Fighter source, short startCellId, short endCellId) {
            this.TurnCount = 0;
        }

        private void Fight_FightStartEvt(Fight obj) {
            this.Fighter.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            this.MakeInvulnerable(this.Fighter, -1);
            this.Fighter.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        private void Fighter_OnTurnStartEvt(Fighter obj) {
            if (this.Invunlnerable) {
                this.TurnCount++;

                if (obj.Stats.MovementPoints.TotalInContext() != obj.Stats.MovementPoints.Total()) {
                    this.TurnCount = 0;
                }

                if (this.TurnCount == TURN_COUNT) {
                    this.Fighter.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                    this.Fighter.Die(this.Fighter);
                    this.Fighter.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                }
            }
        }
    }
}