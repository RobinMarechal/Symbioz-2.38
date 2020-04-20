using Symbioz.Protocol.Selfmade.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using Symbioz.World.Models.Fights.Damages;

namespace Symbioz.World.Providers.Fights.Effects.Damages {
    [SpellEffectHandler(EffectsEnum.Effect_DamageMpPercentageAir)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageMpPercentageNeutral)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageMpPercentageEarth)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageMpPercentageWater)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageMpPercentageFire)]
    public class DamageFromMpPercentage : SpellEffectHandler {
        private EffectElementType ElementType { get; set; }

        public DamageFromMpPercentage(Fighter source,
                                      SpellLevelRecord spellLevel,
                                      EffectInstance effect,
                                      Fighter[] targets,
                                      MapPoint castPoint,
                                      bool critical) : base(source, spellLevel, effect, targets, castPoint, critical) {
            switch (effect.EffectEnum) {
                case EffectsEnum.Effect_DamageMpPercentageFire:
                    this.ElementType = EffectElementType.Fire;

                    break;
                case EffectsEnum.Effect_DamageMpPercentageWater:
                    this.ElementType = EffectElementType.Water;

                    break;
                case EffectsEnum.Effect_DamageMpPercentageEarth:
                    this.ElementType = EffectElementType.Earth;

                    break;
                case EffectsEnum.Effect_DamageMpPercentageAir:
                    this.ElementType = EffectElementType.Air;

                    break;
                case EffectsEnum.Effect_DamageMpPercentageNeutral:
                    this.ElementType = EffectElementType.Neutral;

                    break;
            }
        }

        public override bool Apply(Fighter[] targets) {
            Jet jet = FormulasProvider.Instance.EvaluateJet(this.Source, EffectElementType.Fire, this.Effect, this.SpellId);
            jet.Delta = (short) (jet.Delta * (this.Source.Stats.MpPercentage / 100d));
            foreach (var target in targets) {
                target.InflictDamages(new Damage(this.Source, target, jet.Clone(), EffectElementType.Fire, this.Effect, this.Critical));
            }

            return true;
        }
    }
}