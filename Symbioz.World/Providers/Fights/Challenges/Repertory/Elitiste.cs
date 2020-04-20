using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Fights.Damages;
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
    /// Toutes les attaques doivent être concentrées sur %1 jusqu à ce qu'il meure.
    /// </summary>
    [Challenge(ChallengeTypeEnum.Elitiste)]
    public class Elitiste : Challenge {
        private Fighter Target { get; set; }

        public override int XpBonusPercent {
            get { return 50; }
        }

        public override int DropBonusPercent {
            get { return 50; }
        }

        public Elitiste(ChallengeRecord template, FightTeam team)
            : base(template, team) { }

        public override void Initialize() {
            this.Target = this.Team.OposedTeam().HigherFighter();
            this.Fight.ShowCell(this.Target, (ushort) this.Target.CellId);
            base.Initialize();
        }

        public override void BindEvents() {
            foreach (var fighter in this.Team.OposedTeam().GetFighters()) {
                fighter.BeforeDeadEvt += this.OnDead;
                fighter.OnDamageTaken += this.OnDamageTaken;
            }
        }

        public override void UnBindEvents() {
            foreach (var fighter in this.Team.OposedTeam().GetFighters()) {
                fighter.BeforeDeadEvt -= this.OnDead;
                fighter.OnDamageTaken -= this.OnDamageTaken;
            }
        }

        void OnDamageTaken(Fighter arg1, Damage arg2) {
            if (arg1 != this.Target && arg2.Source.Team == this.Team) {
                this.OnChallengeResulted(ChallengeResultEnum.FAILED);
            }
        }

        void OnDead(Fighter obj) {
            if (obj == this.Target) {
                this.OnChallengeResulted(ChallengeResultEnum.SUCCES);
            }
            else {
                this.OnChallengeResulted(ChallengeResultEnum.FAILED);
            }
        }

        public override bool Valid() {
            return this.Team.OposedTeam().GetFightersCount(false) > 1;
        }

        public override int GetTargetId() {
            return this.Target.Id;
        }

        public override short GetTargetedCell() {
            return this.Target.CellId;
        }
    }
}