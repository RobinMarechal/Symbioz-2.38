using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightTriggerGlyphTrapMessage : AbstractGameActionMessage {
        public const ushort Id = 5741;

        public override ushort MessageId {
            get { return Id; }
        }

        public short markId;
        public double triggeringCharacterId;
        public ushort triggeredSpellId;


        public GameActionFightTriggerGlyphTrapMessage() { }

        public GameActionFightTriggerGlyphTrapMessage(ushort actionId, double sourceId, short markId, double triggeringCharacterId, ushort triggeredSpellId)
            : base(actionId, sourceId) {
            this.markId = markId;
            this.triggeringCharacterId = triggeringCharacterId;
            this.triggeredSpellId = triggeredSpellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.markId);
            writer.WriteDouble(this.triggeringCharacterId);
            writer.WriteVarUhShort(this.triggeredSpellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.markId = reader.ReadShort();
            this.triggeringCharacterId = reader.ReadDouble();

            if (this.triggeringCharacterId < -9007199254740990 || this.triggeringCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on triggeringCharacterId = "
                                    + this.triggeringCharacterId
                                    + ", it doesn't respect the following condition : triggeringCharacterId < -9007199254740990 || triggeringCharacterId > 9007199254740990");
            this.triggeredSpellId = reader.ReadVarUhShort();

            if (this.triggeredSpellId < 0)
                throw new Exception("Forbidden value on triggeredSpellId = " + this.triggeredSpellId + ", it doesn't respect the following condition : triggeredSpellId < 0");
        }
    }
}