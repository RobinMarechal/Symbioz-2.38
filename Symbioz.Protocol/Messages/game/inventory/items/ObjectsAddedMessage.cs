using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectsAddedMessage : Message {
        public const ushort Id = 6033;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem[] @object;


        public ObjectsAddedMessage() { }

        public ObjectsAddedMessage(ObjectItem[] @object) {
            this.@object = @object;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.@object.Length);
            foreach (var entry in this.@object) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.@object = new ObjectItem[limit];
            for (int i = 0; i < limit; i++) {
                this.@object[i] = new ObjectItem();
                this.@object[i].Deserialize(reader);
            }
        }
    }
}