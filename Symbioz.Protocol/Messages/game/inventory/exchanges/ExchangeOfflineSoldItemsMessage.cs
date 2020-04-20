using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeOfflineSoldItemsMessage : Message {
        public const ushort Id = 6613;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemGenericQuantityPrice[] bidHouseItems;
        public ObjectItemGenericQuantityPrice[] merchantItems;


        public ExchangeOfflineSoldItemsMessage() { }

        public ExchangeOfflineSoldItemsMessage(ObjectItemGenericQuantityPrice[] bidHouseItems, ObjectItemGenericQuantityPrice[] merchantItems) {
            this.bidHouseItems = bidHouseItems;
            this.merchantItems = merchantItems;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.bidHouseItems.Length);
            foreach (var entry in this.bidHouseItems) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.merchantItems.Length);
            foreach (var entry in this.merchantItems) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.bidHouseItems = new ObjectItemGenericQuantityPrice[limit];
            for (int i = 0; i < limit; i++) {
                this.bidHouseItems[i] = new ObjectItemGenericQuantityPrice();
                this.bidHouseItems[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.merchantItems = new ObjectItemGenericQuantityPrice[limit];
            for (int i = 0; i < limit; i++) {
                this.merchantItems[i] = new ObjectItemGenericQuantityPrice();
                this.merchantItems[i].Deserialize(reader);
            }
        }
    }
}