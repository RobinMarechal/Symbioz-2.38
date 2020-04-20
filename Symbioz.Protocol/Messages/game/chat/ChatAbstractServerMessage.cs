using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChatAbstractServerMessage : Message {
        public const ushort Id = 880;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte channel;
        public string content;
        public int timestamp;
        public string fingerprint;


        public ChatAbstractServerMessage() { }

        public ChatAbstractServerMessage(sbyte channel, string content, int timestamp, string fingerprint) {
            this.channel = channel;
            this.content = content;
            this.timestamp = timestamp;
            this.fingerprint = fingerprint;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.channel);
            writer.WriteUTF(this.content);
            writer.WriteInt(this.timestamp);
            writer.WriteUTF(this.fingerprint);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.channel = reader.ReadSByte();

            if (this.channel < 0)
                throw new Exception("Forbidden value on channel = " + this.channel + ", it doesn't respect the following condition : channel < 0");
            this.content = reader.ReadUTF();
            this.timestamp = reader.ReadInt();

            if (this.timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + this.timestamp + ", it doesn't respect the following condition : timestamp < 0");
            this.fingerprint = reader.ReadUTF();
        }
    }
}