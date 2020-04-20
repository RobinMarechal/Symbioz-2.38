using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.Protocol.Enums;

namespace Symbioz.World.Providers.Brain.Behaviors.Salbatroces {
    /// <summary>
    /// Rester 5 tours a distance
    /// </summary>
    [Behavior("Bacterrib")]
    public class Bacterrib : SalbatroceBehavior {
        public const int TURN_COUNT = 5;

        public int TurnCount { get; private set; }

        private bool Invunlnerable {
            get { return this.Fighter.HasState(x => x.Invulnerable); }
        }

        public Bacterrib(BrainFighter fighter) : base(fighter) {
            fighter.Fight.FightStartEvt += this.Fight_FightStartEvt;
            this.Fighter.OnTurnStartEvt += this.Fighter_OnTurnStartEvt;
            this.TurnCount = 0;
        }

        private void Fight_FightStartEvt(Models.Fights.Fight obj) {
            this.Fighter.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            this.MakeInvulnerable(this.Fighter, -1);
            this.Fighter.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        private void Fighter_OnTurnStartEvt(Fighter obj) {
            if (this.Invunlnerable) {
                this.TurnCount++;

                if (this.Fighter.Abilities.IsMeleeWithEnenmy()) {
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