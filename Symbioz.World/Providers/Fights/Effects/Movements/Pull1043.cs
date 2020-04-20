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
    [SpellEffectHandler(EffectsEnum.Effect_1043)]
    public class Pull1043 : SpellEffectHandler {
        public Pull1043(Fighter source,
                        SpellLevelRecord spellLevel,
                        EffectInstance effect,
                        Fighter[] targets,
                        MapPoint castPoint,
                        bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            var direction = this.Source.Point.OrientationTo(this.CastPoint);

            MapPoint point = this.Source.Point;

            for (short i = 1; i < this.SpellLevel.MaxRange + 1; i++) {
                point = this.Source.Point.GetCellInDirection(direction, i);

                Fighter target = this.Fight.GetFighter(point.CellId);

                if (target != null) {
                    target.Slide(this.Source, this.Source.Point.GetCellInDirection(direction, 1).CellId);

                    break;
                }
            }

            return true;
        }
    }
}