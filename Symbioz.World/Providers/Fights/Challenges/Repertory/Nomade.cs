using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Records.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Challenges.Repertory {
    /// <summary>
    /// Utiliser tous ses PM disponibles à chaque tour et ne pas se faire tacler pendant toute la durée du combat.
    /// </summary>
    [Challenge(ChallengeTypeEnum.Nomade)]
    public class Nomade : Challenge {
        public override int XpBonusPercent {
            get { return 40; }
        }

        public override int DropBonusPercent {
            get { return 40; }
        }

        public Nomade(ChallengeRecord template, FightTeam team)
            : base(template, team) { }

        void OnTurnEnded(Fighter obj) {
            if (obj.Stats.MovementPoints.TotalInContext() > 0) {
                this.OnChallengeResulted(ChallengeResultEnum.FAILED);
            }
        }

        public override void BindEvents() {
            foreach (var fighter in this.Team.GetFighters(false)) {
                fighter.OnTurnEndEvt += this.OnTurnEnded;
            }
        }

        public override void UnBindEvents() {
            foreach (var fighter in this.Team.GetFighters(false)) {
                fighter.OnTurnEndEvt -= this.OnTurnEnded;
            }
        }
    }
}