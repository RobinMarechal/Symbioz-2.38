using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseUnsoldItemsMessage : Message {
        public const ushort Id = 6612;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemGenericQuantity[] items;


        public ExchangeBidHouseUnsoldItemsMessage() { }

        public ExchangeBidHouseUnsoldItemsMessage(ObjectItemGenericQuantity[] items) {
            this.items = items;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.items.Length);
            foreach (var entry in this.items) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.items = new ObjectItemGenericQuantity[limit];
            for (int i = 0; i < limit; i++) {
                this.items[i] = new ObjectItemGenericQuantity();
                this.items[i].Deserialize(reader);
            }
        }
    }
}