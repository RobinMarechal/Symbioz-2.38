using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using Symbioz.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Actions {
    [Brain(ActionType.MoveToEnemy)]
    public class MoveToEnemyAction : BrainAction {
        private Fighter Target { get; set; }

        public MoveToEnemyAction(BrainFighter fighter)
            : base(fighter) { }

        public override void Analyse() {
            List<Fighter> fighters = this.Fighter.OposedTeam().GetFighters().FindAll(x => !x.IsSummon);
            this.Target = fighters.Count == 0 ? null : fighters.Aggregate((f1, f2) => f1.Stats.CurrentLifePoints < f2.Stats.CurrentLifePoints ? f1 : f2);
        }

        public override void Execute() {
            if (this.Target == null)
                return;
            var path = this.Target.FindPathTo(this.Fighter);

            if (this.Fighter.CellId == this.Target.CellId) {
                var points = this.Target.Point.GetNearPoints();

                var point = points.FirstOrDefault(x => this.Fighter.Fight.IsCellFree(x.CellId));

                if (point != null) this.Fighter.Move(new List<short>() { this.Fighter.CellId, point.CellId});

                return;
            }

            if (path.Count() > 0) this.Fighter.Move(path.ToList());
        }
    }
}