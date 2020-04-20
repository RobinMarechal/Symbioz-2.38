using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectGroundListAddedMessage : Message {
        public const ushort Id = 5925;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] cells;
        public ushort[] referenceIds;


        public ObjectGroundListAddedMessage() { }

        public ObjectGroundListAddedMessage(ushort[] cells, ushort[] referenceIds) {
            this.cells = cells;
            this.referenceIds = referenceIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.cells.Length);
            foreach (var entry in this.cells) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.referenceIds.Length);
            foreach (var entry in this.referenceIds) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.cells = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.cells[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.referenceIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.referenceIds[i] = reader.ReadVarUhShort();
            }
        }
    }
}