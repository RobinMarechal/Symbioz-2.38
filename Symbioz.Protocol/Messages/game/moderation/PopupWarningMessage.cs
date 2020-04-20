using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PopupWarningMessage : Message {
        public const ushort Id = 6134;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte lockDuration;
        public string author;
        public string content;


        public PopupWarningMessage() { }

        public PopupWarningMessage(byte lockDuration, string author, string content) {
            this.lockDuration = lockDuration;
            this.author = author;
            this.content = content;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.lockDuration);
            writer.WriteUTF(this.author);
            writer.WriteUTF(this.content);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.lockDuration = reader.ReadByte();

            if (this.lockDuration < 0 || this.lockDuration > 255)
                throw new Exception("Forbidden value on lockDuration = " + this.lockDuration + ", it doesn't respect the following condition : lockDuration < 0 || lockDuration > 255");
            this.author = reader.ReadUTF();
            this.content = reader.ReadUTF();
        }
    }
}