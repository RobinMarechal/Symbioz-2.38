using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildMemberLeavingMessage : Message {
        public const ushort Id = 5923;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool kicked;
        public ulong memberId;


        public GuildMemberLeavingMessage() { }

        public GuildMemberLeavingMessage(bool kicked, ulong memberId) {
            this.kicked = kicked;
            this.memberId = memberId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.kicked);
            writer.WriteVarUhLong(this.memberId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.kicked = reader.ReadBoolean();
            this.memberId = reader.ReadVarUhLong();

            if (this.memberId < 0 || this.memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + this.memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
        }
    }
}