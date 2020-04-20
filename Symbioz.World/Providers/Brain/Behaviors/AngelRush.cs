using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Behaviors {
    [Behavior("AngelRush")]
    public class AngelRush : Behavior {
        public const ushort SpellId = 7355;

        private bool IsAngel { get; set; }
        private SpellRecord SpellRecord { get; set; }

        public AngelRush(BrainFighter fighter)
            : base(fighter) {
            this.SpellRecord = SpellRecord.GetSpellRecord(SpellId);
            this.Fighter.AfterSlideEvt += this.Fighter_AfterSlideEvt;
            this.Fighter.AfterDeadEvt += this.Fighter_AfterDeadEvt;
            this.Fighter.OnDamageTaken += this.Fighter_OnDamageTaken;
            this.IsAngel = false;
        }

        private void Fighter_OnDamageTaken(Fighter arg1, Models.Fights.Damages.Damage arg2) {
            if (!this.IsAngel) {
                this.Fighter.ForceSpellCast(this.SpellRecord.GetLastLevel(), this.Fighter.CellId);
                this.IsAngel = true;
            }
        }

        private void Fighter_AfterDeadEvt(Fighter obj, bool recursiveCall) {
            if (this.IsAngel) {
                foreach (var ally in this.Fighter.Team.GetFighters()) {
                    ally.ForceSpellCast(this.SpellRecord.GetLastLevel(), ally.CellId);
                }
            }
        }

        private void Fighter_AfterSlideEvt(Fighter target, Fighter source, short startCellId, short endCellId) {
            this.Fighter.ForceSpellCast(this.SpellRecord.GetLastLevel(), this.Fighter.CellId);
            this.IsAngel = true;
        }
    }
}