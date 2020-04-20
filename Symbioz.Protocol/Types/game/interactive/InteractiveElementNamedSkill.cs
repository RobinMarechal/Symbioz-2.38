// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class InteractiveElementNamedSkill : InteractiveElementSkill {
        public const short Id = 220;

        public override short TypeId {
            get { return Id; }
        }

        public uint nameId;


        public InteractiveElementNamedSkill() { }

        public InteractiveElementNamedSkill(uint skillId, int skillInstanceUid, uint nameId)
            : base(skillId, skillInstanceUid) {
            this.nameId = nameId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.nameId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.nameId = reader.ReadVarUhInt();

            if (this.nameId < 0)
                throw new Exception("Forbidden value on nameId = " + this.nameId + ", it doesn't respect the following condition : nameId < 0");
        }
    }
}