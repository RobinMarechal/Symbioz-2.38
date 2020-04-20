// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class SkillActionDescriptionCraft : SkillActionDescription {
        public const short Id = 100;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte probability;


        public SkillActionDescriptionCraft() { }

        public SkillActionDescriptionCraft(ushort skillId, sbyte probability)
            : base(skillId) {
            this.probability = probability;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.probability);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.probability = reader.ReadSByte();

            if (this.probability < 0)
                throw new Exception("Forbidden value on probability = " + this.probability + ", it doesn't respect the following condition : probability < 0");
        }
    }
}