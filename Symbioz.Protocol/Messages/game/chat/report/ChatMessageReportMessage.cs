using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChatMessageReportMessage : Message {
        public const ushort Id = 821;

        public override ushort MessageId {
            get { return Id; }
        }

        public string senderName;
        public string content;
        public int timestamp;
        public sbyte channel;
        public string fingerprint;
        public sbyte reason;


        public ChatMessageReportMessage() { }

        public ChatMessageReportMessage(string senderName, string content, int timestamp, sbyte channel, string fingerprint, sbyte reason) {
            this.senderName = senderName;
            this.content = content;
            this.timestamp = timestamp;
            this.channel = channel;
            this.fingerprint = fingerprint;
            this.reason = reason;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.senderName);
            writer.WriteUTF(this.content);
            writer.WriteInt(this.timestamp);
            writer.WriteSByte(this.channel);
            writer.WriteUTF(this.fingerprint);
            writer.WriteSByte(this.reason);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.senderName = reader.ReadUTF();
            this.content = reader.ReadUTF();
            this.timestamp = reader.ReadInt();

            if (this.timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + this.timestamp + ", it doesn't respect the following condition : timestamp < 0");
            this.channel = reader.ReadSByte();

            if (this.channel < 0)
                throw new Exception("Forbidden value on channel = " + this.channel + ", it doesn't respect the following condition : channel < 0");
            this.fingerprint = reader.ReadUTF();
            this.reason = reader.ReadSByte();

            if (this.reason < 0)
                throw new Exception("Forbidden value on reason = " + this.reason + ", it doesn't respect the following condition : reason < 0");
        }
    }
}