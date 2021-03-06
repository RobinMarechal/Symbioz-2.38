using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareCanceledMessage : Message {
        public const ushort Id = 6679;

        public override ushort MessageId {
            get { return Id; }
        }

        public double dareId;


        public DareCanceledMessage() { }

        public DareCanceledMessage(double dareId) {
            this.dareId = dareId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.dareId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dareId = reader.ReadDouble();

            if (this.dareId < 0 || this.dareId > 9007199254740990)
                throw new Exception("Forbidden value on dareId = " + this.dareId + ", it doesn't respect the following condition : dareId < 0 || dareId > 9007199254740990");
        }
    }
}