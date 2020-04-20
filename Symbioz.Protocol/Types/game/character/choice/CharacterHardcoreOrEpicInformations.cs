// Generated on 04/27/2016 01:13:10

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class CharacterHardcoreOrEpicInformations : CharacterBaseInformations {
        public const short Id = 474;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte deathState;
        public ushort deathCount;
        public byte deathMaxLevel;


        public CharacterHardcoreOrEpicInformations() { }

        public CharacterHardcoreOrEpicInformations(ulong id, string name, byte level, EntityLook entityLook, sbyte breed, bool sex, sbyte deathState, ushort deathCount, byte deathMaxLevel)
            : base(id, name, level, entityLook, breed, sex) {
            this.deathState = deathState;
            this.deathCount = deathCount;
            this.deathMaxLevel = deathMaxLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.deathState);
            writer.WriteVarUhShort(this.deathCount);
            writer.WriteByte(this.deathMaxLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.deathState = reader.ReadSByte();

            if (this.deathState < 0)
                throw new Exception("Forbidden value on deathState = " + this.deathState + ", it doesn't respect the following condition : deathState < 0");
            this.deathCount = reader.ReadVarUhShort();

            if (this.deathCount < 0)
                throw new Exception("Forbidden value on deathCount = " + this.deathCount + ", it doesn't respect the following condition : deathCount < 0");
            this.deathMaxLevel = reader.ReadByte();

            if (this.deathMaxLevel < 1 || this.deathMaxLevel > 200)
                throw new Exception("Forbidden value on deathMaxLevel = " + this.deathMaxLevel + ", it doesn't respect the following condition : deathMaxLevel < 1 || deathMaxLevel > 200");
        }
    }
}