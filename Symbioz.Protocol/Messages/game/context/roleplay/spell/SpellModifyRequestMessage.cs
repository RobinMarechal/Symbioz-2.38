using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SpellModifyRequestMessage : Message {
        public const ushort Id = 6655;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort spellId;
        public sbyte spellLevel;


        public SpellModifyRequestMessage() { }

        public SpellModifyRequestMessage(ushort spellId, sbyte spellLevel) {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.spellId);
            writer.WriteSByte(this.spellLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.spellLevel = reader.ReadSByte();

            if (this.spellLevel < 1 || this.spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + this.spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
    }
}