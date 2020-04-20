using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PaddockSellBuyDialogMessage : Message {
        public const ushort Id = 6018;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool bsell;
        public uint ownerId;
        public uint price;


        public PaddockSellBuyDialogMessage() { }

        public PaddockSellBuyDialogMessage(bool bsell, uint ownerId, uint price) {
            this.bsell = bsell;
            this.ownerId = ownerId;
            this.price = price;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.bsell);
            writer.WriteVarUhInt(this.ownerId);
            writer.WriteVarUhInt(this.price);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.bsell = reader.ReadBoolean();
            this.ownerId = reader.ReadVarUhInt();

            if (this.ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + this.ownerId + ", it doesn't respect the following condition : ownerId < 0");
            this.price = reader.ReadVarUhInt();

            if (this.price < 0)
                throw new Exception("Forbidden value on price = " + this.price + ", it doesn't respect the following condition : price < 0");
        }
    }
}