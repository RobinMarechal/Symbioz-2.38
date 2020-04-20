using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PaddockMoveItemRequestMessage : Message {
        public const ushort Id = 6052;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort oldCellId;
        public ushort newCellId;


        public PaddockMoveItemRequestMessage() { }

        public PaddockMoveItemRequestMessage(ushort oldCellId, ushort newCellId) {
            this.oldCellId = oldCellId;
            this.newCellId = newCellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.oldCellId);
            writer.WriteVarUhShort(this.newCellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.oldCellId = reader.ReadVarUhShort();

            if (this.oldCellId < 0 || this.oldCellId > 559)
                throw new Exception("Forbidden value on oldCellId = " + this.oldCellId + ", it doesn't respect the following condition : oldCellId < 0 || oldCellId > 559");
            this.newCellId = reader.ReadVarUhShort();

            if (this.newCellId < 0 || this.newCellId > 559)
                throw new Exception("Forbidden value on newCellId = " + this.newCellId + ", it doesn't respect the following condition : newCellId < 0 || newCellId > 559");
        }
    }
}