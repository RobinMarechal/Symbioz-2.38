using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StorageObjectRemoveMessage : Message {
        public const ushort Id = 5648;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;


        public StorageObjectRemoveMessage() { }

        public StorageObjectRemoveMessage(uint objectUID) {
            this.objectUID = objectUID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
        }
    }
}