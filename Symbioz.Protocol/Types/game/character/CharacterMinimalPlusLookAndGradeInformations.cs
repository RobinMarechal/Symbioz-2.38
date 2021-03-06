// Generated on 04/27/2016 01:13:09

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations {
        public const short Id = 193;

        public override short TypeId {
            get { return Id; }
        }

        public uint grade;


        public CharacterMinimalPlusLookAndGradeInformations() { }

        public CharacterMinimalPlusLookAndGradeInformations(ulong id, string name, byte level, EntityLook entityLook, uint grade)
            : base(id, name, level, entityLook) {
            this.grade = grade;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.grade);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.grade = reader.ReadVarUhInt();

            if (this.grade < 0)
                throw new Exception("Forbidden value on grade = " + this.grade + ", it doesn't respect the following condition : grade < 0");
        }
    }
}