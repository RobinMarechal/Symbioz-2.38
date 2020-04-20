using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectsQuantityMessage : Message {
        public const ushort Id = 6206;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemQuantity[] objectsUIDAndQty;


        public ObjectsQuantityMessage() { }

        public ObjectsQuantityMessage(ObjectItemQuantity[] objectsUIDAndQty) {
            this.objectsUIDAndQty = objectsUIDAndQty;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objectsUIDAndQty.Length);
            foreach (var entry in this.objectsUIDAndQty) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objectsUIDAndQty = new ObjectItemQuantity[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsUIDAndQty[i] = new ObjectItemQuantity();
                this.objectsUIDAndQty[i].Deserialize(reader);
            }
        }
    }
}