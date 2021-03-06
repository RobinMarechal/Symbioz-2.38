using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectGroundRemovedMultipleMessage : Message {
        public const ushort Id = 5944;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] cells;


        public ObjectGroundRemovedMultipleMessage() { }

        public ObjectGroundRemovedMultipleMessage(ushort[] cells) {
            this.cells = cells;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.cells.Length);
            foreach (var entry in this.cells) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.cells = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.cells[i] = reader.ReadVarUhShort();
            }
        }
    }
}