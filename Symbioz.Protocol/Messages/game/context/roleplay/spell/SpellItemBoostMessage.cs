using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SpellItemBoostMessage : Message {
        public const ushort Id = 6011;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint statId;
        public ushort spellId;
        public short value;


        public SpellItemBoostMessage() { }

        public SpellItemBoostMessage(uint statId, ushort spellId, short value) {
            this.statId = statId;
            this.spellId = spellId;
            this.value = value;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.statId);
            writer.WriteVarUhShort(this.spellId);
            writer.WriteVarShort(this.value);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.statId = reader.ReadVarUhInt();

            if (this.statId < 0)
                throw new Exception("Forbidden value on statId = " + this.statId + ", it doesn't respect the following condition : statId < 0");
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.value = reader.ReadVarShort();
        }
    }
}