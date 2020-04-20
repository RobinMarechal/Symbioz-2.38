using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class OnConnectionEventMessage : Message {
        public const ushort Id = 5726;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte eventType;


        public OnConnectionEventMessage() { }

        public OnConnectionEventMessage(sbyte eventType) {
            this.eventType = eventType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.eventType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.eventType = reader.ReadSByte();

            if (this.eventType < 0)
                throw new Exception("Forbidden value on eventType = " + this.eventType + ", it doesn't respect the following condition : eventType < 0");
        }
    }
}