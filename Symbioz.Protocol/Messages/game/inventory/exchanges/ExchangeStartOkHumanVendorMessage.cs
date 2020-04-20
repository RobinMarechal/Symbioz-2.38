using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkHumanVendorMessage : Message {
        public const ushort Id = 5767;

        public override ushort MessageId {
            get { return Id; }
        }

        public double sellerId;
        public ObjectItemToSellInHumanVendorShop[] objectsInfos;


        public ExchangeStartOkHumanVendorMessage() { }

        public ExchangeStartOkHumanVendorMessage(double sellerId, ObjectItemToSellInHumanVendorShop[] objectsInfos) {
            this.sellerId = sellerId;
            this.objectsInfos = objectsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.sellerId);
            writer.WriteUShort((ushort) this.objectsInfos.Length);
            foreach (var entry in this.objectsInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.sellerId = reader.ReadDouble();

            if (this.sellerId < -9007199254740990 || this.sellerId > 9007199254740990)
                throw new Exception("Forbidden value on sellerId = " + this.sellerId + ", it doesn't respect the following condition : sellerId < -9007199254740990 || sellerId > 9007199254740990");
            var limit = reader.ReadUShort();
            this.objectsInfos = new ObjectItemToSellInHumanVendorShop[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsInfos[i] = new ObjectItemToSellInHumanVendorShop();
                this.objectsInfos[i].Deserialize(reader);
            }
        }
    }
}