using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicTimeMessage : Message {
        public const ushort Id = 175;

        public override ushort MessageId {
            get { return Id; }
        }

        public double timestamp;
        public short timezoneOffset;


        public BasicTimeMessage() { }

        public BasicTimeMessage(double timestamp, short timezoneOffset) {
            this.timestamp = timestamp;
            this.timezoneOffset = timezoneOffset;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.timestamp);
            writer.WriteShort(this.timezoneOffset);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.timestamp = reader.ReadDouble();

            if (this.timestamp < 0 || this.timestamp > 9007199254740990)
                throw new Exception("Forbidden value on timestamp = " + this.timestamp + ", it doesn't respect the following condition : timestamp < 0 || timestamp > 9007199254740990");
            this.timezoneOffset = reader.ReadShort();
        }
    }
}