using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Effects.Summons {
    [SpellEffectHandler(EffectsEnum.Effect_KillReplacePerControlableInvocation)]
    public class ReplacePerControlableInvocation : SpellEffectHandler {
        public ReplacePerControlableInvocation(Fighter source,
                                               SpellLevelRecord level,
                                               EffectInstance effect,
                                               Fighter[] targets,
                                               MapPoint castPoint,
                                               bool critical)
            : base(source, level, effect, targets, castPoint, critical) { }

        public override bool Apply(Fighter[] targets) {
            if (targets.Count() > 0) {
                short[] cells = new short[targets.Length];

                for (int i = 0; i < targets.Length; i++) {
                    targets[i].Stats.CurrentLifePoints = 0;
                    cells[i] = targets[i].CellId;
                }


                this.Source.Fight.CheckDeads();


                foreach (var cell in cells) {
                    ControlableMonsterFighter fighter = this.CreateSummon(this.Source as CharacterFighter);
                    this.Fight.AddSummon(fighter, (CharacterFighter) this.Source);

                    return true;
                }
            }

            return true;
        }

        void fighter_OnDeadEvt(Fighter fighter) {
            // la poupée redevient un arbre ;) en fonction du targetMask selector
        }

        private ControlableMonsterFighter CreateSummon(CharacterFighter master) {
            MonsterRecord template = MonsterRecord.GetMonster(this.Effect.DiceMin);
            sbyte gradeId = (sbyte) (template.GradeExist(this.SpellLevel.Grade) ? this.SpellLevel.Grade : template.LastGrade().Id);

            return new ControlableMonsterFighter(this.Source.Team, template, gradeId, master, this.CastPoint.CellId);
        }
    }
}