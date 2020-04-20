using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Behaviors {
    [Behavior("Protect")]
    public class Protect : Behavior {
        public Protect(BrainFighter fighter)
            : base(fighter) {
            fighter.BeforeDeadEvt += this.fighter_OnDeadEvt;
            fighter.Fight.OnFighters<CharacterFighter>(x => x.Character.Notification("Protegez " + this.Fighter.Name + "!"));
        }

        void fighter_OnDeadEvt(Fighter fighter) {
            foreach (var ally in this.Fighter.Team.GetFighters()) {
                ally.Die(this.Fighter);
            }

            this.Fighter.Fight.CheckDeads();
        }
    }
}