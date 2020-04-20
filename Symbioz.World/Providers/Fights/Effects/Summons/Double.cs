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

namespace Symbioz.World.Providers.Fights.Effects.Summons {
    [SpellEffectHandler(EffectsEnum.Effect_Double)]
    public class Double : SpellEffectHandler {
        public Double(Fighter source,
                      SpellLevelRecord spellLevel,
                      EffectInstance effect,
                      Fighter[] targets,
                      MapPoint castPoint,
                      bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            if (this.Source is CharacterFighter) {
                DoubleFighter fighter = new DoubleFighter((CharacterFighter) this.Source, this.Source.Team, this.CastPoint.CellId);
                this.Fight.AddSummon(fighter, (CharacterFighter) this.Source);

                return true;
            }
            else {
                this.Fight.Reply("An non character fighter try to summon a double...");

                return false;
            }
        }
    }
}