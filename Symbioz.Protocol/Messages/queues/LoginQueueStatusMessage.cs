using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LoginQueueStatusMessage : Message {
        public const ushort Id = 10;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort position;
        public ushort total;


        public LoginQueueStatusMessage() { }

        public LoginQueueStatusMessage(ushort position, ushort total) {
            this.position = position;
            this.total = total;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort(this.position);
            writer.WriteUShort(this.total);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.position = reader.ReadUShort();

            if (this.position < 0 || this.position > 65535)
                throw new Exception("Forbidden value on position = " + this.position + ", it doesn't respect the following condition : position < 0 || position > 65535");
            this.total = reader.ReadUShort();

            if (this.total < 0 || this.total > 65535)
                throw new Exception("Forbidden value on total = " + this.total + ", it doesn't respect the following condition : total < 0 || total > 65535");
        }
    }
}