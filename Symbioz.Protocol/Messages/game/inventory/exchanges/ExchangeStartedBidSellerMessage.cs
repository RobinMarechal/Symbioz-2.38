using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartedBidSellerMessage : Message {
        public const ushort Id = 5905;

        public override ushort MessageId {
            get { return Id; }
        }

        public SellerBuyerDescriptor sellerDescriptor;
        public ObjectItemToSellInBid[] objectsInfos;


        public ExchangeStartedBidSellerMessage() { }

        public ExchangeStartedBidSellerMessage(SellerBuyerDescriptor sellerDescriptor, ObjectItemToSellInBid[] objectsInfos) {
            this.sellerDescriptor = sellerDescriptor;
            this.objectsInfos = objectsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.sellerDescriptor.Serialize(writer);
            writer.WriteUShort((ushort) this.objectsInfos.Length);
            foreach (var entry in this.objectsInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.sellerDescriptor = new SellerBuyerDescriptor();
            this.sellerDescriptor.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.objectsInfos = new ObjectItemToSellInBid[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsInfos[i] = new ObjectItemToSellInBid();
                this.objectsInfos[i].Deserialize(reader);
            }
        }
    }
}