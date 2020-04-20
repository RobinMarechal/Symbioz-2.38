using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismsListRegisterMessage : Message {
        public const ushort Id = 6441;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte listen;


        public PrismsListRegisterMessage() { }

        public PrismsListRegisterMessage(sbyte listen) {
            this.listen = listen;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.listen);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.listen = reader.ReadSByte();

            if (this.listen < 0)
                throw new Exception("Forbidden value on listen = " + this.listen + ", it doesn't respect the following condition : listen < 0");
        }
    }
}