using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeTypesItemsExchangerDescriptionForUserMessage : Message {
        public const ushort Id = 5752;

        public override ushort MessageId {
            get { return Id; }
        }

        public BidExchangerObjectInfo[] itemTypeDescriptions;


        public ExchangeTypesItemsExchangerDescriptionForUserMessage() { }

        public ExchangeTypesItemsExchangerDescriptionForUserMessage(BidExchangerObjectInfo[] itemTypeDescriptions) {
            this.itemTypeDescriptions = itemTypeDescriptions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.itemTypeDescriptions.Length);
            foreach (var entry in this.itemTypeDescriptions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.itemTypeDescriptions = new BidExchangerObjectInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.itemTypeDescriptions[i] = new BidExchangerObjectInfo();
                this.itemTypeDescriptions[i].Deserialize(reader);
            }
        }
    }
}