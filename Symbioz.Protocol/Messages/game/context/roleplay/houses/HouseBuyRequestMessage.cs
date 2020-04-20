using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseBuyRequestMessage : Message {
        public const ushort Id = 5738;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint proposedPrice;


        public HouseBuyRequestMessage() { }

        public HouseBuyRequestMessage(uint proposedPrice) {
            this.proposedPrice = proposedPrice;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.proposedPrice);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.proposedPrice = reader.ReadVarUhInt();

            if (this.proposedPrice < 0)
                throw new Exception("Forbidden value on proposedPrice = " + this.proposedPrice + ", it doesn't respect the following condition : proposedPrice < 0");
        }
    }
}