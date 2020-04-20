using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportOnSameMapMessage : Message {
        public const ushort Id = 6048;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public ushort cellId;


        public TeleportOnSameMapMessage() { }

        public TeleportOnSameMapMessage(double targetId, ushort cellId) {
            this.targetId = targetId;
            this.cellId = cellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhShort(this.cellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.cellId = reader.ReadVarUhShort();

            if (this.cellId < 0 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
    }
}