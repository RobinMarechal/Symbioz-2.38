using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Spells;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Movements {
    [SpellEffectHandler(EffectsEnum.Effect_SymetricTargetTeleport)]
    [SpellEffectHandler(EffectsEnum.Effect_SymetricPointTeleport)]
    public class SymetricPointTeleport : SpellEffectHandler {
        public SymetricPointTeleport(Fighter source,
                                     SpellLevelRecord spellLevel,
                                     EffectInstance effect,
                                     Fighter[] targets,
                                     MapPoint castPoint,
                                     bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            if (this.Fight.GetFighter(this.CastPoint.CellId) != null) {
                Fighter target1 = targets.FirstOrDefault();
                MapPoint point = this.CastPoint;

                point = new MapPoint((2 * this.CastPoint.X - this.Source.Point.X), (2 * this.CastPoint.Y - this.Source.Point.Y));


                if (target1 != null) {
                    Fighter target2 = this.Fight.GetFighter(point);

                    if (target2 == null) {
                        if (this.Fight.IsCellFree(point.CellId)) this.Source.Teleport(this.Source, point);
                    }
                    else {
                        this.Source.SwitchPosition(target2);
                    }

                    return true;
                }

                return false;
            }
            else {
                return false;
            }
        }
    }
}