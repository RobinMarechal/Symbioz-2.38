using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities.Stats;
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

namespace Symbioz.World.Providers.Fights.Effects.Buffs {
    [SpellEffectHandler(EffectsEnum.Effect_Punishment)]
    public class Punishement : SpellEffectHandler {
        public Punishement(Fighter source,
                           SpellLevelRecord spellLevel,
                           EffectInstance effect,
                           Fighter[] targets,
                           MapPoint castPoint,
                           bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            foreach (var target in targets) {
                this.AddTriggerBuff(target, FightDispellableEnum.REALLY_NOT_DISPELLABLE, TriggerType.AFTER_ATTACKED, this.OnActorAttacked);
            }

            return true;
        }

        private bool OnActorAttacked(TriggerBuff buff, TriggerType trigger, object token) {
            IEnumerable<StatBuff> source = buff.Target.GetBuffs<StatBuff>(x => x.SpellId == this.SpellId);
            int num = (
                          from entry in source
                          where (int) entry.Duration == this.Effect.Duration
                          select entry).Sum((StatBuff entry) => (int) entry.Value);

            short diceFace = (short) this.Effect.DiceMax;

            if (num < diceFace) {
                Damage damage = (Damage) token;
                int num2 = damage.Delta;
                if (num2 + num > diceFace) {
                    num2 = (short) (diceFace - num);
                }

                Characteristic characteristic = this.GetPunishementCharacteristic(buff.Target, this.Effect.DiceMin);

                StatBuff buff2 = new StatBuff(buff.Target.BuffIdProvider.Pop(),
                                              buff.Target,
                                              this.Source,
                                              this.SpellLevel,
                                              this.Effect,
                                              this.SpellId,
                                              (short) num2,
                                              characteristic,
                                              false,
                                              FightDispellableEnum.REALLY_NOT_DISPELLABLE,
                                              (short) this.GetPunishementEffectEnum(this.Effect.DiceMin)) {
                    Duration = (short) this.Effect.Value,
                };

                buff.Target.AddAndApplyBuff(buff2, true);
            }

            return false;
        }

        private Characteristic GetPunishementCharacteristic(Fighter fighter, ushort statId) {
            switch (statId) {
                case 118:
                    return fighter.Stats.Strength;
                case 119:
                    return fighter.Stats.Agility;
                case 123:
                    return fighter.Stats.Chance;
                case 124:
                    return fighter.Stats.Wisdom;
                case 125:
                    return fighter.Stats.Vitality; // to see
                case 126:
                    return fighter.Stats.Intelligence;
                case 407:
                    return fighter.Stats.Vitality; // to see
                default:
                    this.Fight.Reply("statId (" + statId + ") is not considered as a punishement characteristic.");

                    return null;
            }
        }

        private EffectsEnum GetPunishementEffectEnum(ushort statId) {
            switch (statId) {
                case 118:
                    return EffectsEnum.Effect_AddStrength;
                case 119:
                    return EffectsEnum.Effect_AddAgility;
                case 123:
                    return EffectsEnum.Effect_AddChance;
                case 124:
                    return EffectsEnum.Effect_AddWisdom;
                case 125:
                    return EffectsEnum.Effect_AddVitality;
                case 126:
                    return EffectsEnum.Effect_AddIntelligence;
                case 407:
                    return EffectsEnum.Effect_AddVitality;
                default:
                    throw new Exception("statId (" + statId + ") is not considered as a punishement characteristic.");
            }
        }
    }
}