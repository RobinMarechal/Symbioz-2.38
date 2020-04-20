using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Core;
using Symbioz.World.Models.Fights.Damages;

namespace Symbioz.World.Providers.Fights.Effects.Damages {
    [SpellEffectHandler(EffectsEnum.Effect_DamageHPLostPercentAir)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageHPLostPercentFire)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageHPLostPercentNeutral)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageHPLostPercentStrenght)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageHPLostPercentWater)]
    public class DamageHPLostPercent : SpellEffectHandler {
        public EffectElementType ElementType { get; set; }

        public DamageHPLostPercent(Fighter source,
                                   SpellLevelRecord spellLevel,
                                   EffectInstance effect,
                                   Fighter[] targets,
                                   MapPoint castPoint,
                                   bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) {
            switch (effect.EffectEnum) {
                case EffectsEnum.Effect_DamageHPLostPercentStrenght:
                    this.ElementType = EffectElementType.Earth;

                    break;
                case EffectsEnum.Effect_DamageHPLostPercentWater:
                    this.ElementType = EffectElementType.Water;

                    break;
                case EffectsEnum.Effect_DamageHPLostPercentFire:
                    this.ElementType = EffectElementType.Fire;

                    break;
                case EffectsEnum.Effect_DamageHPLostPercentAir:
                    this.ElementType = EffectElementType.Air;

                    break;
                case EffectsEnum.Effect_DamageHPLostPercentNeutral:
                    this.ElementType = EffectElementType.Neutral;

                    break;
            }
        }

        public override bool Apply(Fighter[] targets) {
            foreach (var target in targets) {
                short num = (short) target.Stats.LifeLost.GetPercentageOf(this.Effect.DiceMin);
                if (num > 500)
                    num = 500;
                target.InflictDamages(new Damage(this.Source, target, num, this.ElementType));
            }

            return true;
        }
    }
}