using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismSetSabotagedRequestMessage : Message {
        public const ushort Id = 6468;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;


        public PrismSetSabotagedRequestMessage() { }

        public PrismSetSabotagedRequestMessage(ushort subAreaId) {
            this.subAreaId = subAreaId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
    }
}