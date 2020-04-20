using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeSellMessage : Message {
        public const ushort Id = 5778;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectToSellId;
        public uint quantity;


        public ExchangeSellMessage() { }

        public ExchangeSellMessage(uint objectToSellId, uint quantity) {
            this.objectToSellId = objectToSellId;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectToSellId);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectToSellId = reader.ReadVarUhInt();

            if (this.objectToSellId < 0)
                throw new Exception("Forbidden value on objectToSellId = " + this.objectToSellId + ", it doesn't respect the following condition : objectToSellId < 0");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}