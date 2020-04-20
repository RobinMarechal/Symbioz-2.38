﻿using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Spells;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Maps.Shapes;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks {
    public class AuraGlyph : Mark, IDurationMark {
        public override GameActionMarkTypeEnum Type {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        public const string TriggerRawZone = "P1";

        public short Duration { get; set; }

        public override bool BreakMove {
            get { return false; }
        }

        public override GameActionMark GetGameActionMark() {
            return new GameActionMark(this.Source.Id,
                                      this.Source.Team.Id,
                                      this.SpellLevel.SpellId,
                                      this.SpellLevel.Grade,
                                      this.Id,
                                      (sbyte) this.Type,
                                      this.CenterPoint.CellId,
                                      (from entry in this.Shapes
                                       select entry.GetGameActionMarkedCell()).ToArray(),
                                      true);
        }

        public AuraGlyph(short id,
                         Fighter source,
                         SpellLevelRecord spellLevel,
                         EffectInstance effect,
                         MapPoint centerPoint,
                         Zone zone,
                         Color color,
                         MarkTriggerTypeEnum triggerType)
            : base(id, source, spellLevel, effect, centerPoint, zone, color, triggerType) {
            this.Duration = (short) effect.Duration;
        }

        public bool DecrementDuration() {
            return this.Duration-- <= 0;
        }

        public override void Trigger(Fighter source, MarkTriggerTypeEnum type, object token) {
            bool seq = this.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            SpellLevelRecord triggerLevel = this.TriggerSpell.GetLevel((sbyte) this.BaseEffect.DiceMax);
            var effects = new List<EffectInstance>(triggerLevel.Effects);
            effects.Reverse();
            SpellEffectsManager.Instance.HandleEffects(this.Source, effects.ToArray(), triggerLevel, source.Point, TriggerRawZone, false);

            if (seq) this.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        public void Activate(Fighter source) {
            bool seq = this.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            SpellLevelRecord triggerLevel = this.TriggerSpell.GetLevel(this.SpellLevel.Grade);
            SpellEffectsManager.Instance.HandleEffects(this.Source, triggerLevel, this.CenterPoint, false);
            if (seq) this.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }
    }
}