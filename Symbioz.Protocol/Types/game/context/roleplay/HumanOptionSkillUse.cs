// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HumanOptionSkillUse : HumanOption {
        public const short Id = 495;

        public override short TypeId {
            get { return Id; }
        }

        public uint elementId;
        public ushort skillId;
        public double skillEndTime;


        public HumanOptionSkillUse() { }

        public HumanOptionSkillUse(uint elementId, ushort skillId, double skillEndTime) {
            this.elementId = elementId;
            this.skillId = skillId;
            this.skillEndTime = skillEndTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.elementId);
            writer.WriteVarUhShort(this.skillId);
            writer.WriteDouble(this.skillEndTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.elementId = reader.ReadVarUhInt();

            if (this.elementId < 0)
                throw new Exception("Forbidden value on elementId = " + this.elementId + ", it doesn't respect the following condition : elementId < 0");
            this.skillId = reader.ReadVarUhShort();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
            this.skillEndTime = reader.ReadDouble();

            if (this.skillEndTime < -9007199254740990 || this.skillEndTime > 9007199254740990)
                throw new Exception("Forbidden value on skillEndTime = " + this.skillEndTime + ", it doesn't respect the following condition : skillEndTime < -9007199254740990 || skillEndTime > 9007199254740990");
        }
    }
}