using Symbioz.Protocol.Enums;
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

namespace Symbioz.World.Providers.Fights.Effects.Buffs {
    [SpellEffectHandler(EffectsEnum.Effect_AddAP_111), SpellEffectHandler(EffectsEnum.Effect_RegainAP)]
    public class ApBuff : SpellEffectHandler {
        public ApBuff(Fighter source,
                      SpellLevelRecord spellLevel,
                      EffectInstance effect,
                      Fighter[] targets,
                      MapPoint castPoint,
                      bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            foreach (var fighter in targets) {
                if (this.Effect.Duration > 0) {
                    this.AddStatBuff(fighter, (short) this.Effect.DiceMin, fighter.Stats.ActionPoints, FightDispellableEnum.DISPELLABLE);
                }
                else {
                    fighter.RegainAp(this.Source.Id, (short) this.Effect.DiceMin);
                }
            }

            return true;
        }
    }
}