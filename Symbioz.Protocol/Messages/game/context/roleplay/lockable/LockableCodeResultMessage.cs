using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LockableCodeResultMessage : Message {
        public const ushort Id = 5672;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte result;


        public LockableCodeResultMessage() { }

        public LockableCodeResultMessage(sbyte result) {
            this.result = result;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.result);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.result = reader.ReadSByte();

            if (this.result < 0)
                throw new Exception("Forbidden value on result = " + this.result + ", it doesn't respect the following condition : result < 0");
        }
    }
}