using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismFightSwapRequestMessage : Message {
        public const ushort Id = 5901;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;
        public ulong targetId;


        public PrismFightSwapRequestMessage() { }

        public PrismFightSwapRequestMessage(ushort subAreaId, ulong targetId) {
            this.subAreaId = subAreaId;
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteVarUhLong(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.targetId = reader.ReadVarUhLong();

            if (this.targetId < 0 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
        }
    }
}