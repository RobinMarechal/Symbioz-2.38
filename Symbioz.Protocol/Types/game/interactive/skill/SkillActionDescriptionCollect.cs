// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class SkillActionDescriptionCollect : SkillActionDescriptionTimed {
        public const short Id = 99;
        public override short TypeId => Id;

        public ushort min;
        public ushort max;


        public SkillActionDescriptionCollect() { }

        public SkillActionDescriptionCollect(ushort skillId, byte time, ushort min, ushort max)
            : base(skillId, time) {
            this.min = min;
            this.max = max;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.min);
            writer.WriteVarUhShort(this.max);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.min = reader.ReadVarUhShort();

            if (this.min < 0)
                throw new Exception("Forbidden value on min = " + this.min + ", it doesn't respect the following condition : min < 0");
            this.max = reader.ReadVarUhShort();

            if (this.max < 0)
                throw new Exception("Forbidden value on max = " + this.max + ", it doesn't respect the following condition : max < 0");
        }
    }
}