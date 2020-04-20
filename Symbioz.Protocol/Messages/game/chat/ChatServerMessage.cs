using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChatServerMessage : ChatAbstractServerMessage {
        public const ushort Id = 881;

        public override ushort MessageId {
            get { return Id; }
        }

        public double senderId;
        public string senderName;
        public int senderAccountId;


        public ChatServerMessage() { }

        public ChatServerMessage(sbyte channel, string content, int timestamp, string fingerprint, double senderId, string senderName, int senderAccountId)
            : base(channel, content, timestamp, fingerprint) {
            this.senderId = senderId;
            this.senderName = senderName;
            this.senderAccountId = senderAccountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.senderId);
            writer.WriteUTF(this.senderName);
            writer.WriteInt(this.senderAccountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.senderId = reader.ReadDouble();

            if (this.senderId < -9007199254740990 || this.senderId > 9007199254740990)
                throw new Exception("Forbidden value on senderId = " + this.senderId + ", it doesn't respect the following condition : senderId < -9007199254740990 || senderId > 9007199254740990");
            this.senderName = reader.ReadUTF();
            this.senderAccountId = reader.ReadInt();

            if (this.senderAccountId < 0)
                throw new Exception("Forbidden value on senderAccountId = " + this.senderAccountId + ", it doesn't respect the following condition : senderAccountId < 0");
        }
    }
}