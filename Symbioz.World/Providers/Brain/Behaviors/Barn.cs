using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Core;
using Symbioz.World.Models.Fights;

namespace Symbioz.World.Providers.Brain.Behaviors {
    [Behavior("Barn")]
    public class Barn : Behavior {
        public const ushort SummonedMonsterId = 671;

        private MonsterRecord SummonedTemplate { get; set; }
        private AsyncRandom GradeRandomizer { get; set; }

        public Barn(BrainFighter fighter)
            : base(fighter) {
            this.GradeRandomizer = new AsyncRandom();
            fighter.Fight.FightStartEvt += this.Fight_FightStart;

            fighter.OnDamageTaken += this.fighter_OnDamageTaken;

            this.SummonedTemplate = MonsterRecord.GetMonster(SummonedMonsterId);
        }

        void fighter_OnDamageTaken(Fighter fighter, Damage obj) {
            List<Fighter> fighters = this.Fighter.Fight.GetAllFighters();
            fighters.Remove(this.Fighter);
            fighters.Random().Heal(this.Fighter, obj.Delta);
        }

        void Fight_FightStart(Fight fight) {
            CharacterFighter master = this.Fighter.OposedTeam().GetFighters<CharacterFighter>().FirstOrDefault();

            this.Fighter.AfterSlideEvt += this.Fighter_OnSlideEvt;
        }

        void Fighter_OnSlideEvt(Fighter target, Fighter source, short startCellId, short endCellId) {
            SummonedFighter summon = this.CreateSummoned(source);

            this.Fighter.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            this.Fighter.Fight.AddSummon(summon);
            this.Fighter.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        private SummonedFighter CreateSummoned(Fighter master) {
            return new SummonedFighter(this.SummonedTemplate, (sbyte) this.GradeRandomizer.Next(1, 5), master, master.Team, this.Fighter.Fight.RandomFreeCell());
        }
    }
}