using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MailStatusMessage : Message {
        public const ushort Id = 6275;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort unread;
        public ushort total;


        public MailStatusMessage() { }

        public MailStatusMessage(ushort unread, ushort total) {
            this.unread = unread;
            this.total = total;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.unread);
            writer.WriteVarUhShort(this.total);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.unread = reader.ReadVarUhShort();

            if (this.unread < 0)
                throw new Exception("Forbidden value on unread = " + this.unread + ", it doesn't respect the following condition : unread < 0");
            this.total = reader.ReadVarUhShort();

            if (this.total < 0)
                throw new Exception("Forbidden value on total = " + this.total + ", it doesn't respect the following condition : total < 0");
        }
    }
}