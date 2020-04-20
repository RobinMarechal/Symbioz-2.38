using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;

namespace Symbioz.World.Providers.Brain.Behaviors.Salbatroces {
    public abstract class SalbatroceBehavior : Behavior {
        public const ushort MalamibeId = 3822;

        public SalbatroceBehavior(BrainFighter fighter) : base(fighter) {
            this.Fighter.OnTurnStartEvt += this.Fighter_OnTurnStartEvt;
        }

        private void Fighter_OnTurnStartEvt(Fighter obj) {
            if (!this.MalimbeExist() && this.Fighter.Alive) {
                var summon = this.Summon(this.Fighter, MalamibeId, obj.NearFreeCell());
                summon.Brain.GetBehavior<Malamibe>().OnSummoned();
            }
        }

        private bool MalimbeExist() {
            return this.Fighter.Team.GetFighters<BrainFighter>().FirstOrDefault(x => x.MonsterId == MalamibeId) != null;
        }
    }
}