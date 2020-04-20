using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Damages {
    [SpellEffectHandler(EffectsEnum.Effect_StealHPFire)]
    [SpellEffectHandler(EffectsEnum.Effect_StealHPWater)]
    [SpellEffectHandler(EffectsEnum.Effect_StealHPEarth)]
    [SpellEffectHandler(EffectsEnum.Effect_StealHPAir)]
    [SpellEffectHandler(EffectsEnum.Effect_StealHPNeutral)]
    [SpellEffectHandler(EffectsEnum.Effect_StealHPFix)]
    public class LifeSteal : SpellEffectHandler {
        public EffectElementType ElementType { get; set; }

        public LifeSteal(Fighter source,
                         SpellLevelRecord spellLevel,
                         EffectInstance effect,
                         Fighter[] targets,
                         MapPoint castPoint,
                         bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) {
            switch (effect.EffectEnum) {
                case EffectsEnum.Effect_StealHPEarth:
                    this.ElementType = EffectElementType.Earth;

                    break;
                case EffectsEnum.Effect_StealHPWater:
                    this.ElementType = EffectElementType.Water;

                    break;
                case EffectsEnum.Effect_StealHPFire:
                    this.ElementType = EffectElementType.Fire;

                    break;
                case EffectsEnum.Effect_StealHPAir:
                    this.ElementType = EffectElementType.Air;

                    break;
                case EffectsEnum.Effect_StealHPNeutral:
                    this.ElementType = EffectElementType.Neutral;

                    break;
                case EffectsEnum.Effect_StealHPFix:
                    this.ElementType = EffectElementType.Neutral; // todo, neutral but no jet

                    break;
            }
        }

        public override bool Apply(Fighter[] targets) {
            if (this.ElementType != EffectElementType.Neutral) {
                Jet jet = FormulasProvider.Instance.EvaluateJet(this.Source, this.ElementType, this.Effect, this.SpellLevel.SpellId);

                foreach (var target in targets) {
                    short num = (short) (target.InflictDamages(new Damage(this.Source, target, jet.Clone(), this.ElementType, this.Effect, this.Critical)) / 2);
                    this.Source.Heal(this.Source, num);
                }

                return true;
            }
            else {
                foreach (var target in targets) {
                    short num = (short) (target.InflictDamages((short) this.Effect.DiceMin, this.Source) / 2);
                    this.Source.Heal(this.Source, num);
                }

                return true;
            }
        }
    }
}