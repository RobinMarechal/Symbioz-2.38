using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseSellRequestMessage : Message {
        public const ushort Id = 5697;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint amount;
        public bool forSale;


        public HouseSellRequestMessage() { }

        public HouseSellRequestMessage(uint amount, bool forSale) {
            this.amount = amount;
            this.forSale = forSale;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.amount);
            writer.WriteBoolean(this.forSale);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.amount = reader.ReadVarUhInt();

            if (this.amount < 0)
                throw new Exception("Forbidden value on amount = " + this.amount + ", it doesn't respect the following condition : amount < 0");
            this.forSale = reader.ReadBoolean();
        }
    }
}