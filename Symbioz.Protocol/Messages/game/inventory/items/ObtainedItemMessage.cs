using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObtainedItemMessage : Message {
        public const ushort Id = 6519;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort genericId;
        public uint baseQuantity;


        public ObtainedItemMessage() { }

        public ObtainedItemMessage(ushort genericId, uint baseQuantity) {
            this.genericId = genericId;
            this.baseQuantity = baseQuantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.genericId);
            writer.WriteVarUhInt(this.baseQuantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.genericId = reader.ReadVarUhShort();

            if (this.genericId < 0)
                throw new Exception("Forbidden value on genericId = " + this.genericId + ", it doesn't respect the following condition : genericId < 0");
            this.baseQuantity = reader.ReadVarUhInt();

            if (this.baseQuantity < 0)
                throw new Exception("Forbidden value on baseQuantity = " + this.baseQuantity + ", it doesn't respect the following condition : baseQuantity < 0");
        }
    }
}