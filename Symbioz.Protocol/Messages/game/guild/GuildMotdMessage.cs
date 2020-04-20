using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildMotdMessage : Message {
        public const ushort Id = 6590;

        public override ushort MessageId {
            get { return Id; }
        }

        public string content;
        public int timestamp;
        public ulong memberId;
        public string memberName;


        public GuildMotdMessage() { }

        public GuildMotdMessage(string content, int timestamp, ulong memberId, string memberName) {
            this.content = content;
            this.timestamp = timestamp;
            this.memberId = memberId;
            this.memberName = memberName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.content);
            writer.WriteInt(this.timestamp);
            writer.WriteVarUhLong(this.memberId);
            writer.WriteUTF(this.memberName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.content = reader.ReadUTF();
            this.timestamp = reader.ReadInt();

            if (this.timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + this.timestamp + ", it doesn't respect the following condition : timestamp < 0");
            this.memberId = reader.ReadVarUhLong();

            if (this.memberId < 0 || this.memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + this.memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            this.memberName = reader.ReadUTF();
        }
    }
}