using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ConsoleMessage : Message {
        public const ushort Id = 75;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte type;
        public string content;


        public ConsoleMessage() { }

        public ConsoleMessage(sbyte type, string content) {
            this.type = type;
            this.content = content;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.type);
            writer.WriteUTF(this.content);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.type = reader.ReadSByte();

            if (this.type < 0)
                throw new Exception("Forbidden value on type = " + this.type + ", it doesn't respect the following condition : type < 0");
            this.content = reader.ReadUTF();
        }
    }
}