// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class InteractiveElementSkill {
        public const short Id = 219;

        public virtual short TypeId {
            get { return Id; }
        }

        public uint skillId;
        public int skillInstanceUid;


        public InteractiveElementSkill() { }

        public InteractiveElementSkill(uint skillId, int skillInstanceUid) {
            this.skillId = skillId;
            this.skillInstanceUid = skillInstanceUid;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.skillId);
            writer.WriteInt(this.skillInstanceUid);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.skillId = reader.ReadVarUhInt();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
            this.skillInstanceUid = reader.ReadInt();

            if (this.skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + this.skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
    }
}