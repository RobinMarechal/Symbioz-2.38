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
    [SpellEffectHandler(EffectsEnum.Effect_DamageAirPerAP)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageEarthPerAP)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageFirePerAP)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageNeutralPerAP)]
    [SpellEffectHandler(EffectsEnum.Effect_DamageWaterPerAP)]
    public class DamagePerApUsed : SpellEffectHandler {
        private EffectElementType ElementType { get; set; }

        public DamagePerApUsed(Fighter source,
                               SpellLevelRecord level,
                               EffectInstance effect,
                               Fighter[] targets,
                               MapPoint castPoint,
                               bool critical)
            : base(source, level, effect, targets, castPoint, critical) {
            switch (this.Effect.EffectEnum) {
                case EffectsEnum.Effect_DamageAirPerAP:
                    this.ElementType = EffectElementType.Air;

                    break;
                case EffectsEnum.Effect_DamageEarthPerAP:
                    this.ElementType = EffectElementType.Earth;

                    break;
                case EffectsEnum.Effect_DamageFirePerAP:
                    this.ElementType = EffectElementType.Fire;

                    break;
                case EffectsEnum.Effect_DamageNeutralPerAP:
                    this.ElementType = EffectElementType.Neutral;

                    break;
                case EffectsEnum.Effect_DamageWaterPerAP:
                    this.ElementType = EffectElementType.Water;

                    break;
            }
        }

        public override bool Apply(Fighter[] targets) {
            foreach (var target in targets) {
                this.AddTriggerBuff(target, FightDispellableEnum.DISPELLABLE, TriggerType.TURN_END, this.OnTurnEnded);
            }

            return true;
        }

        private bool OnTurnEnded(TriggerBuff buff, TriggerType trigger, object token) {
            short jetMin = (short) (this.Effect.DiceMin * buff.Target.Stats.ApUsed);
            short jetMax = (short) (this.Effect.DiceMax * buff.Target.Stats.ApUsed);

            if (jetMin < jetMax) {
                Jet jet = FormulasProvider.Instance.EvaluateJet(buff.Caster, this.ElementType, jetMin, jetMax, buff.Caster.GetSpellBoost(this.SpellId), false);
                buff.Target.InflictDamages(new Damage(buff.Caster, buff.Target, jet, this.ElementType, this.Effect, this.Critical));
            }

            return false;
        }
    }
}