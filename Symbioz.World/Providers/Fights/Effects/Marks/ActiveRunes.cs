using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Marks {
    [SpellEffectHandler(EffectsEnum.Effect_ActiveRunes)]
    public class ActiveRunes : SpellEffectHandler {
        public ActiveRunes(Fighter source,
                           SpellLevelRecord level,
                           EffectInstance effect,
                           Fighter[] targets,
                           MapPoint castPoint,
                           bool critical)
            : base(source, level, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            foreach (var rune in this.Fight.GetMarks<Rune>(x => x.Source == this.Source)) {
                rune.Activate(this.Source);
            }

            return true;
        }
    }
}