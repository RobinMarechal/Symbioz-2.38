using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CheckFileMessage : Message {
        public const ushort Id = 6156;

        public override ushort MessageId {
            get { return Id; }
        }

        public string filenameHash;
        public sbyte type;
        public string value;


        public CheckFileMessage() { }

        public CheckFileMessage(string filenameHash, sbyte type, string value) {
            this.filenameHash = filenameHash;
            this.type = type;
            this.value = value;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.filenameHash);
            writer.WriteSByte(this.type);
            writer.WriteUTF(this.value);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.filenameHash = reader.ReadUTF();
            this.type = reader.ReadSByte();

            if (this.type < 0)
                throw new Exception("Forbidden value on type = " + this.type + ", it doesn't respect the following condition : type < 0");
            this.value = reader.ReadUTF();
        }
    }
}