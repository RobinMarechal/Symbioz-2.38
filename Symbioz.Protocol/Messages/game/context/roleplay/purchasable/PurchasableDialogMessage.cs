using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PurchasableDialogMessage : Message {
        public const ushort Id = 5739;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool buyOrSell;
        public uint purchasableId;
        public uint price;


        public PurchasableDialogMessage() { }

        public PurchasableDialogMessage(bool buyOrSell, uint purchasableId, uint price) {
            this.buyOrSell = buyOrSell;
            this.purchasableId = purchasableId;
            this.price = price;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.buyOrSell);
            writer.WriteVarUhInt(this.purchasableId);
            writer.WriteVarUhInt(this.price);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.buyOrSell = reader.ReadBoolean();
            this.purchasableId = reader.ReadVarUhInt();

            if (this.purchasableId < 0)
                throw new Exception("Forbidden value on purchasableId = " + this.purchasableId + ", it doesn't respect the following condition : purchasableId < 0");
            this.price = reader.ReadVarUhInt();

            if (this.price < 0)
                throw new Exception("Forbidden value on price = " + this.price + ", it doesn't respect the following condition : price < 0");
        }
    }
}