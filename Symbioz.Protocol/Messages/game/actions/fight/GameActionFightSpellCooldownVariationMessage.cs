using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightSpellCooldownVariationMessage : AbstractGameActionMessage {
        public const ushort Id = 6219;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public ushort spellId;
        public short value;


        public GameActionFightSpellCooldownVariationMessage() { }

        public GameActionFightSpellCooldownVariationMessage(ushort actionId, double sourceId, double targetId, ushort spellId, short value)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.spellId = spellId;
            this.value = value;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhShort(this.spellId);
            writer.WriteVarShort(this.value);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.value = reader.ReadVarShort();
        }
    }
}