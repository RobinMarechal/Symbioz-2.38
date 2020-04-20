using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InteractiveUseEndedMessage : Message {
        public const ushort Id = 6112;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint elemId;
        public ushort skillId;


        public InteractiveUseEndedMessage() { }

        public InteractiveUseEndedMessage(uint elemId, ushort skillId) {
            this.elemId = elemId;
            this.skillId = skillId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.elemId);
            writer.WriteVarUhShort(this.skillId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.elemId = reader.ReadVarUhInt();

            if (this.elemId < 0)
                throw new Exception("Forbidden value on elemId = " + this.elemId + ", it doesn't respect the following condition : elemId < 0");
            this.skillId = reader.ReadVarUhShort();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
        }
    }
}