using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Providers.Fights.Buffs;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Damages {
    [SpellEffectHandler(EffectsEnum.Effect_DamageFire)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageEarth)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageAir)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageWater)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageNeutral)]
    public class DirectDamage : SpellEffectHandler {
        public EffectElementType ElementType { get; set; }

        public DirectDamage(Fighter source,
                            SpellLevelRecord spellLevel,
                            EffectInstance effect,
                            Fighter[] targets,
                            MapPoint castPoint,
                            bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) {
            switch (effect.EffectEnum) {
                case EffectsEnum.Effect_DamageEarth:
                    this.ElementType = EffectElementType.Earth;

                    break;
                case EffectsEnum.Effect_DamageWater:
                    this.ElementType = EffectElementType.Water;

                    break;
                case EffectsEnum.Effect_DamageFire:
                    this.ElementType = EffectElementType.Fire;

                    break;
                case EffectsEnum.Effect_DamageAir:
                    this.ElementType = EffectElementType.Air;

                    break;
                case EffectsEnum.Effect_DamageNeutral:
                    this.ElementType = EffectElementType.Neutral;

                    break;
            }
        }

        public override bool Apply(Fighter[] targets) {
            if (this.Effect.Duration > 0) {
                foreach (var target in targets) {
                    this.AddTriggerBuff(target, FightDispellableEnum.DISPELLABLE, TriggerType.TURN_BEGIN, this.DamageTrigger);
                }
            }
            else {
                Jet jet = FormulasProvider.Instance.EvaluateJet(this.Source, this.ElementType, this.Effect, this.SpellId);

                foreach (var target in targets) {
                    target.InflictDamages(new Damage(this.Source, target, jet.Clone(), this.ElementType, this.Effect, this.Critical));
                }
            }

            return true;
        }

        private bool DamageTrigger(TriggerBuff buff, TriggerType trigger, object token) {
            Jet jet = FormulasProvider.Instance.EvaluateJet(this.Source, this.ElementType, this.Effect, this.SpellId);
            buff.Target.InflictDamages(new Damage(this.Source, buff.Target, jet, this.ElementType, this.Effect, this.Critical));

            return false;
        }
    }
}