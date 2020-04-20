using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PaddockSellRequestMessage : Message {
        public const ushort Id = 5953;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint price;
        public bool forSale;


        public PaddockSellRequestMessage() { }

        public PaddockSellRequestMessage(uint price, bool forSale) {
            this.price = price;
            this.forSale = forSale;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.price);
            writer.WriteBoolean(this.forSale);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.price = reader.ReadVarUhInt();

            if (this.price < 0)
                throw new Exception("Forbidden value on price = " + this.price + ", it doesn't respect the following condition : price < 0");
            this.forSale = reader.ReadBoolean();
        }
    }
}