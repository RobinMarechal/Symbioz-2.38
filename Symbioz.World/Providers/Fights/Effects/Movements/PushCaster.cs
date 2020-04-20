﻿using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Providers.Maps.Path;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Movements {
    [SpellEffectHandler(EffectsEnum.Eff_PushCaster)]
    public class PushCaster : SpellEffectHandler {
        public PushCaster(Fighter source,
                          SpellLevelRecord spellLevel,
                          EffectInstance effect,
                          Fighter[] targets,
                          MapPoint castPoint,
                          bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            foreach (var target in targets) {
                target.Abilities.PushCaster(this.Source, (short) this.Effect.DiceMin, this.CastPoint);
            }

            return true;
        }
    }
}