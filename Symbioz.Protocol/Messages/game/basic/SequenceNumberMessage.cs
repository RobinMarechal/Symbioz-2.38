using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SequenceNumberMessage : Message {
        public const ushort Id = 6317;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort number;


        public SequenceNumberMessage() { }

        public SequenceNumberMessage(ushort number) {
            this.number = number;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort(this.number);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.number = reader.ReadUShort();

            if (this.number < 0 || this.number > 65535)
                throw new Exception("Forbidden value on number = " + this.number + ", it doesn't respect the following condition : number < 0 || number > 65535");
        }
    }
}