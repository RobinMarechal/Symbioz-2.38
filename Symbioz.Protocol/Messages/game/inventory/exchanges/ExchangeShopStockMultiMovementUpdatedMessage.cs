using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeShopStockMultiMovementUpdatedMessage : Message {
        public const ushort Id = 6038;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemToSell[] objectInfoList;


        public ExchangeShopStockMultiMovementUpdatedMessage() { }

        public ExchangeShopStockMultiMovementUpdatedMessage(ObjectItemToSell[] objectInfoList) {
            this.objectInfoList = objectInfoList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objectInfoList.Length);
            foreach (var entry in this.objectInfoList) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objectInfoList = new ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++) {
                this.objectInfoList[i] = new ObjectItemToSell();
                this.objectInfoList[i].Deserialize(reader);
            }
        }
    }
}