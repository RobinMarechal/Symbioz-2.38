using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Behaviors {
    [Behavior("Pusher")]
    public class Pusher : Behavior {
        public const short PushAmount = 2;

        public Pusher(BrainFighter fighter)
            : base(fighter) {
            this.Fighter.OnTurnStartEvt += this.Fighter_OnTurnStartEvt;
        }

        void Fighter_OnTurnStartEvt(Fighter obj) {
            foreach (var fighter in this.Fighter.OposedTeam().GetFighters()) {
                fighter.Abilities.PushBack(this.Fighter, PushAmount, this.Fighter.Point);
            }
        }
    }
}