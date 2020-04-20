using Symbioz.Core;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain.Actions {
    [Brain(ActionType.CastSpell)]
    public class CastSpellAction : BrainAction {
        private Dictionary<int, SpellCategoryEnum> Categories { get; set; }

        public CastSpellAction(BrainFighter fighter)
            : base(fighter) { }

        public override void Analyse() {
            this.Categories = EnvironmentAnalyser.Instance.GetSpellsCategories(this.Fighter);
        }

        public override void Execute() {
            if (!this.Fighter.Alive)
                return;

            foreach (var category in this.Categories.OrderByDescending(x => x.Key).Reverse()) {
                var levels = this.Fighter.Template.SpellRecords.FindAll(x => x.CategoryEnum == category.Value).ConvertAll<SpellLevelRecord>(x => x.GetLastLevel());

                foreach (var level in levels.Shuffle()) {
                    if (this.Fighter.Stats.ActionPoints.TotalInContext() >= level.ApCost) {
                        if (this.Fighter.Fight.Ended)
                            return;

                        short cellId = EnvironmentAnalyser.Instance.GetTargetedCell(this.Fighter, category.Value, level);

                        if (cellId != -1) {
                            var spell = SpellRecord.GetSpellRecord(level.SpellId);
                            if (spell != null) this.Fighter.CastSpell(spell, spell.GetLastLevelGrade(), cellId);
                        }
                        else
                            break;
                    }
                }
            }
        }
    }
}