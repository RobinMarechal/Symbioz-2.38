using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartedBidBuyerMessage : Message {
        public const ushort Id = 5904;

        public override ushort MessageId {
            get { return Id; }
        }

        public SellerBuyerDescriptor buyerDescriptor;


        public ExchangeStartedBidBuyerMessage() { }

        public ExchangeStartedBidBuyerMessage(SellerBuyerDescriptor buyerDescriptor) {
            this.buyerDescriptor = buyerDescriptor;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.buyerDescriptor.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.buyerDescriptor = new SellerBuyerDescriptor();
            this.buyerDescriptor.Deserialize(reader);
        }
    }
}