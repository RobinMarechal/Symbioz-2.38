using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ShowCellMessage : Message {
        public const ushort Id = 5612;

        public override ushort MessageId {
            get { return Id; }
        }

        public double sourceId;
        public ushort cellId;


        public ShowCellMessage() { }

        public ShowCellMessage(double sourceId, ushort cellId) {
            this.sourceId = sourceId;
            this.cellId = cellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.sourceId);
            writer.WriteVarUhShort(this.cellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.sourceId = reader.ReadDouble();

            if (this.sourceId < -9007199254740990 || this.sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + this.sourceId + ", it doesn't respect the following condition : sourceId < -9007199254740990 || sourceId > 9007199254740990");
            this.cellId = reader.ReadVarUhShort();

            if (this.cellId < 0 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
    }
}