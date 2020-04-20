using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using Symbioz.World.Models.Fights.Spells;
using Symbioz.World.Models.Effects;

namespace Symbioz.World.Providers.Fights.Spells.Roublard {
    [CustomSpellHandler(2801)] // Aimantation 
    public class Magnetization : CustomSpellHandler {
        public Magnetization(Fighter source, SpellLevelRecord level, MapPoint castPoint, bool criticalHit) : base(source, level, castPoint, criticalHit) { }

        public override void Execute() {
            EffectInstance[] effects = this.GetEffects();
            this.HandlePullAlliesEnemies(effects);
            this.HandlePullBombs(effects);
        }

        private void HandlePullAlliesEnemies(EffectInstance[] effects) {
            var zone = effects[0].GetZone(this.Source.Point.OrientationTo(this.CastPoint));
            var targets = SpellEffectsManager.Instance.GetAffectedFighters(this.Source, zone, this.CastPoint, effects[0].TargetMask).ToList();
            targets.Remove(this.Source);
            targets.Remove(this.Source.Fight.GetFighter(this.CastPoint));
            this.Handler(effects[0], this.CastPoint, targets.ToArray());
        }

        private void HandlePullBombs(EffectInstance[] effects) {
            var zone = effects[1].GetZone(this.Source.Point.OrientationTo(this.CastPoint));
            var targets = SpellEffectsManager.Instance.GetAffectedFighters(this.Source, zone, this.CastPoint, effects[1].TargetMask).ToList();
            targets.Remove(this.Source.Fight.GetFighter(this.CastPoint));
            this.Handler(effects[1], this.CastPoint, targets.ToArray());
        }
    }
}