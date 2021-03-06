using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeShopStockMovementUpdatedMessage : Message {
        public const ushort Id = 5909;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemToSell objectInfo;


        public ExchangeShopStockMovementUpdatedMessage() { }

        public ExchangeShopStockMovementUpdatedMessage(ObjectItemToSell objectInfo) {
            this.objectInfo = objectInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.objectInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectInfo = new ObjectItemToSell();
            this.objectInfo.Deserialize(reader);
        }
    }
}