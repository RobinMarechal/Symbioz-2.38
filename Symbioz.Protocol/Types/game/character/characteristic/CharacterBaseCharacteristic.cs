// Generated on 04/27/2016 01:13:09

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class CharacterBaseCharacteristic {
        public const short Id = 4;

        public virtual short TypeId {
            get { return Id; }
        }

        public short @base;
        public short additionnal;
        public short objectsAndMountBonus;
        public short alignGiftBonus;
        public short contextModif;


        public CharacterBaseCharacteristic() { }

        public CharacterBaseCharacteristic(short @base, short additionnal, short objectsAndMountBonus, short alignGiftBonus, short contextModif) {
            this.@base = @base;
            this.additionnal = additionnal;
            this.objectsAndMountBonus = objectsAndMountBonus;
            this.alignGiftBonus = alignGiftBonus;
            this.contextModif = contextModif;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarShort(this.@base);
            writer.WriteVarShort(this.additionnal);
            writer.WriteVarShort(this.objectsAndMountBonus);
            writer.WriteVarShort(this.alignGiftBonus);
            writer.WriteVarShort(this.contextModif);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.@base = reader.ReadVarShort();
            this.additionnal = reader.ReadVarShort();
            this.objectsAndMountBonus = reader.ReadVarShort();
            this.alignGiftBonus = reader.ReadVarShort();
            this.contextModif = reader.ReadVarShort();
        }
    }
}