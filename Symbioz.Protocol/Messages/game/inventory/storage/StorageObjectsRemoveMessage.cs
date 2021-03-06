using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StorageObjectsRemoveMessage : Message {
        public const ushort Id = 6035;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint[] objectUIDList;


        public StorageObjectsRemoveMessage() { }

        public StorageObjectsRemoveMessage(uint[] objectUIDList) {
            this.objectUIDList = objectUIDList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objectUIDList.Length);
            foreach (var entry in this.objectUIDList) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objectUIDList = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.objectUIDList[i] = reader.ReadVarUhInt();
            }
        }
    }
}