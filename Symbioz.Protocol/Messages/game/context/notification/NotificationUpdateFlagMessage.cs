using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NotificationUpdateFlagMessage : Message {
        public const ushort Id = 6090;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort index;


        public NotificationUpdateFlagMessage() { }

        public NotificationUpdateFlagMessage(ushort index) {
            this.index = index;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.index);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.index = reader.ReadVarUhShort();

            if (this.index < 0)
                throw new Exception("Forbidden value on index = " + this.index + ", it doesn't respect the following condition : index < 0");
        }
    }
}