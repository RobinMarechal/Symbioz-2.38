using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseItemAddOkMessage : Message {
        public const ushort Id = 5945;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemToSellInBid itemInfo;


        public ExchangeBidHouseItemAddOkMessage() { }

        public ExchangeBidHouseItemAddOkMessage(ObjectItemToSellInBid itemInfo) {
            this.itemInfo = itemInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.itemInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.itemInfo = new ObjectItemToSellInBid();
            this.itemInfo.Deserialize(reader);
        }
    }
}