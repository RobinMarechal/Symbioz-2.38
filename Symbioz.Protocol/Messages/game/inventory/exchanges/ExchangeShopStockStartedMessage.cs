using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeShopStockStartedMessage : Message {
        public const ushort Id = 5910;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemToSell[] objectsInfos;


        public ExchangeShopStockStartedMessage() { }

        public ExchangeShopStockStartedMessage(ObjectItemToSell[] objectsInfos) {
            this.objectsInfos = objectsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objectsInfos.Length);
            foreach (var entry in this.objectsInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objectsInfos = new ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsInfos[i] = new ObjectItemToSell();
                this.objectsInfos[i].Deserialize(reader);
            }
        }
    }
}