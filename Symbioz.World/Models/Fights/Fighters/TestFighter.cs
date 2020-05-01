using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Records.Monsters;

namespace Symbioz.World.Models.Fights.Fighters {
    class TestFighter : BrainFighter, ISummon<Fighter> {
        public Fighter Owner { get; set; }
        private short SummonCellId { get; set; }

        public TestFighter(MonsterRecord template, sbyte gradeId, Fighter owner, FightTeam team, short cellId)
            : base(team, 0, template, gradeId) {
            this.Owner = owner;
            this.SummonCellId = cellId;
        }

        public bool IsOwner(Fighter fighter) {
            return this.Owner == fighter;
        }

        public override bool Sex {
            get { return false; }
        }

        public override string Name {
            get { return this.Template.Name; }
        }

        public override ushort Level {
            get { return this.Grade.Level; }
        }

        public override void Initialize() {
            base.Initialize();
            this.CellId = this.SummonCellId;
            this.FightStartCell = this.SummonCellId;
            this.Direction = this.Owner.Point.OrientationTo(this.Point, false);
            this.Stats.InitializeSummon(this.Owner, true);
        }

        public override void OnTurnStarted() {
            base.OnTurnStarted();
            if (!this.Fight.Ended) this.PassTurn();
        }

        public override bool InsertInTimeline() {
            return this.Template.UseSummonSlot;
        }

        public override GameFightFighterInformations GetFightFighterInformations() {
            return new GameFightMonsterInformations(this.Id,
                                                    this.Look.ToEntityLook(),
                                                    new EntityDispositionInformations(this.CellId, (sbyte) this.Direction),
                                                    this.Team.Id,
                                                    0,
                                                    this.Alive,
                                                    this.Stats.GetFightMinimalStats(),
                                                    new ushort[0],
                                                    this.Template.Id,
                                                    this.GradeId);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformation() {
            throw new NotImplementedException();
        }
    }
}