using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InteractiveUseErrorMessage : Message {
        public const ushort Id = 6384;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint elemId;
        public uint skillInstanceUid;


        public InteractiveUseErrorMessage() { }

        public InteractiveUseErrorMessage(uint elemId, uint skillInstanceUid) {
            this.elemId = elemId;
            this.skillInstanceUid = skillInstanceUid;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.elemId);
            writer.WriteVarUhInt(this.skillInstanceUid);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.elemId = reader.ReadVarUhInt();

            if (this.elemId < 0)
                throw new Exception("Forbidden value on elemId = " + this.elemId + ", it doesn't respect the following condition : elemId < 0");
            this.skillInstanceUid = reader.ReadVarUhInt();

            if (this.skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + this.skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
    }
}