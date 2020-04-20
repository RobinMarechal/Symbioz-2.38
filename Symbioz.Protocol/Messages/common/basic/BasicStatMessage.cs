using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicStatMessage : Message {
        public const ushort Id = 6530;

        public override ushort MessageId {
            get { return Id; }
        }

        public double timeSpent;
        public ushort statId;


        public BasicStatMessage() { }

        public BasicStatMessage(double timeSpent, ushort statId) {
            this.timeSpent = timeSpent;
            this.statId = statId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.timeSpent);
            writer.WriteVarUhShort(this.statId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.timeSpent = reader.ReadDouble();

            if (this.timeSpent < 0 || this.timeSpent > 9007199254740990)
                throw new Exception("Forbidden value on timeSpent = " + this.timeSpent + ", it doesn't respect the following condition : timeSpent < 0 || timeSpent > 9007199254740990");
            this.statId = reader.ReadVarUhShort();

            if (this.statId < 0)
                throw new Exception("Forbidden value on statId = " + this.statId + ", it doesn't respect the following condition : statId < 0");
        }
    }
}