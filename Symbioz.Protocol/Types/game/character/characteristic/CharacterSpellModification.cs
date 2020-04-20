// Generated on 04/27/2016 01:13:10

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class CharacterSpellModification {
        public const short Id = 215;

        public virtual short TypeId {
            get { return Id; }
        }

        public sbyte modificationType;
        public ushort spellId;
        public CharacterBaseCharacteristic value;


        public CharacterSpellModification() { }

        public CharacterSpellModification(sbyte modificationType, ushort spellId, CharacterBaseCharacteristic value) {
            this.modificationType = modificationType;
            this.spellId = spellId;
            this.value = value;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.modificationType);
            writer.WriteVarUhShort(this.spellId);
            this.value.Serialize(writer);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.modificationType = reader.ReadSByte();

            if (this.modificationType < 0)
                throw new Exception("Forbidden value on modificationType = " + this.modificationType + ", it doesn't respect the following condition : modificationType < 0");
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.value = new CharacterBaseCharacteristic();
            this.value.Deserialize(reader);
        }
    }
}