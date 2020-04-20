using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Maps.Shapes;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Marks {
    [SpellEffectHandler(EffectsEnum.Effect_Trap)]
    public class TrapSpawn : SpellEffectHandler {
        public TrapSpawn(Fighter source,
                         SpellLevelRecord spellLevel,
                         EffectInstance effect,
                         Fighter[] targets,
                         MapPoint castPoint,
                         bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            Zone zone = new Zone(this.Effect.ShapeType, this.Effect.Radius);
            Color color = Color.FromArgb(this.Effect.Value);
            Trap trap = new Trap(this.Fight.PopNextMarkId(), this.Source, this.SpellLevel, this.Effect, this.CastPoint, zone, color);
            this.Fight.AddMark(trap);

            return true;
        }
    }
}