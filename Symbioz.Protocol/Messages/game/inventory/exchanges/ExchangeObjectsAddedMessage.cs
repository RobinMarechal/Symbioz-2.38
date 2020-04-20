using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeObjectsAddedMessage : ExchangeObjectMessage {
        public const ushort Id = 6535;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem[] @object;


        public ExchangeObjectsAddedMessage() { }

        public ExchangeObjectsAddedMessage(bool remote, ObjectItem[] @object)
            : base(remote) {
            this.@object = @object;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.@object.Length);
            foreach (var entry in this.@object) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.@object = new ObjectItem[limit];
            for (int i = 0; i < limit; i++) {
                this.@object[i] = new ObjectItem();
                this.@object[i].Deserialize(reader);
            }
        }
    }
}