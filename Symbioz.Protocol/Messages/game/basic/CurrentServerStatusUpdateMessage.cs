using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CurrentServerStatusUpdateMessage : Message {
        public const ushort Id = 6525;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte status;


        public CurrentServerStatusUpdateMessage() { }

        public CurrentServerStatusUpdateMessage(sbyte status) {
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.status);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.status = reader.ReadSByte();

            if (this.status < 0)
                throw new Exception("Forbidden value on status = " + this.status + ", it doesn't respect the following condition : status < 0");
        }
    }
}