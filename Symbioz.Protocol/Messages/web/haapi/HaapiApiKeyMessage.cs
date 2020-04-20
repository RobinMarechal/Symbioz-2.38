using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HaapiApiKeyMessage : Message {
        public const ushort Id = 6649;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte keyType;
        public string token;


        public HaapiApiKeyMessage() { }

        public HaapiApiKeyMessage(sbyte keyType, string token) {
            this.keyType = keyType;

            this.token = token;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.keyType);
            writer.WriteUTF(this.token);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.keyType = reader.ReadSByte();

            if (this.keyType < 0)
                throw new Exception("Forbidden value on keyType = " + this.keyType + ", it doesn't respect the following condition : keyType < 0");
            this.token = reader.ReadUTF();
        }
    }
}