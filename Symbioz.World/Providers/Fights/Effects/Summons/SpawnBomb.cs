using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Spells;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Summons {
    [SpellEffectHandler(EffectsEnum.Effect_1008)]
    public class SpawnBomb : SpellEffectHandler {
        public SpawnBomb(Fighter source,
                         SpellLevelRecord spellLevel,
                         EffectInstance effect,
                         Fighter[] targets,
                         MapPoint castPoint,
                         bool critical)
            : base(source, spellLevel, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            var target = this.Fight.GetFighter(this.CastPoint);

            if (target != null) {
                SpellBombRecord record = SpellBombRecord.GetSpellBombRecord(this.SpellLevel.SpellId);
                var level = SpellRecord.GetSpellRecord(record.CibleExplosionSpellId).GetLevel(this.SpellLevel.Grade);
                this.Source.ForceSpellCast(level, this.CastPoint.CellId);
            }
            else {
                MonsterRecord record = MonsterRecord.GetMonster(this.Effect.DiceMin);
                BombFighter fighter = new BombFighter(record, this.Source, this.Source.Team, this.CastPoint.CellId, this.SpellLevel.Grade, this.SpellLevel);
                this.Fight.AddBomb(fighter, this.Source);
            }

            return true;
        }
    }
}