using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectsDeletedMessage : Message {
        public const ushort Id = 6034;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint[] objectUID;


        public ObjectsDeletedMessage() { }

        public ObjectsDeletedMessage(uint[] objectUID) {
            this.objectUID = objectUID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objectUID.Length);
            foreach (var entry in this.objectUID) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objectUID = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.objectUID[i] = reader.ReadVarUhInt();
            }
        }
    }
}