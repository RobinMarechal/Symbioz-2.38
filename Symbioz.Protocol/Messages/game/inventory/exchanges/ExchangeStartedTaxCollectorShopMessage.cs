using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartedTaxCollectorShopMessage : Message {
        public const ushort Id = 6664;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem[] objects;
        public uint kamas;


        public ExchangeStartedTaxCollectorShopMessage() { }

        public ExchangeStartedTaxCollectorShopMessage(ObjectItem[] objects, uint kamas) {
            this.objects = objects;
            this.kamas = kamas;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objects.Length);
            foreach (var entry in this.objects) {
                entry.Serialize(writer);
            }

            writer.WriteVarUhInt(this.kamas);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objects = new ObjectItem[limit];
            for (int i = 0; i < limit; i++) {
                this.objects[i] = new ObjectItem();
                this.objects[i].Deserialize(reader);
            }

            this.kamas = reader.ReadVarUhInt();

            if (this.kamas < 0)
                throw new Exception("Forbidden value on kamas = " + this.kamas + ", it doesn't respect the following condition : kamas < 0");
        }
    }
}