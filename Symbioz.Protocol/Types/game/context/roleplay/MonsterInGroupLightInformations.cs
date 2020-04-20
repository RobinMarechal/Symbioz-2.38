// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class MonsterInGroupLightInformations {
        public const short Id = 395;

        public virtual short TypeId {
            get { return Id; }
        }

        public int creatureGenericId;
        public sbyte grade;


        public MonsterInGroupLightInformations() { }

        public MonsterInGroupLightInformations(int creatureGenericId, sbyte grade) {
            this.creatureGenericId = creatureGenericId;
            this.grade = grade;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.creatureGenericId);
            writer.WriteSByte(this.grade);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.creatureGenericId = reader.ReadInt();
            this.grade = reader.ReadSByte();

            if (this.grade < 0)
                throw new Exception("Forbidden value on grade = " + this.grade + ", it doesn't respect the following condition : grade < 0");
        }
    }
}