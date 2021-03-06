using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectUseOnCellMessage : ObjectUseMessage {
        public const ushort Id = 3013;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort cells;


        public ObjectUseOnCellMessage() { }

        public ObjectUseOnCellMessage(uint objectUID, ushort cells)
            : base(objectUID) {
            this.cells = cells;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.cells);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.cells = reader.ReadVarUhShort();

            if (this.cells < 0 || this.cells > 559)
                throw new Exception("Forbidden value on cells = " + this.cells + ", it doesn't respect the following condition : cells < 0 || cells > 559");
        }
    }
}