using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StorageObjectUpdateMessage : Message {
        public const ushort Id = 5647;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem @object;


        public StorageObjectUpdateMessage() { }

        public StorageObjectUpdateMessage(ObjectItem @object) {
            this.@object = @object;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.@object.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.@object = new ObjectItem();
            this.@object.Deserialize(reader);
        }
    }
}