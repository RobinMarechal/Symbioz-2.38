using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DebugHighlightCellsMessage : Message {
        public const ushort Id = 2001;

        public override ushort MessageId {
            get { return Id; }
        }

        public int color;
        public ushort[] cells;


        public DebugHighlightCellsMessage() { }

        public DebugHighlightCellsMessage(int color, ushort[] cells) {
            this.color = color;
            this.cells = cells;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.color);
            writer.WriteUShort((ushort) this.cells.Length);
            foreach (var entry in this.cells) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.color = reader.ReadInt();
            var limit = reader.ReadUShort();
            this.cells = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.cells[i] = reader.ReadVarUhShort();
            }
        }
    }
}