using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInvitationMessage : Message {
        public const ushort Id = 5551;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong targetId;


        public GuildInvitationMessage() { }

        public GuildInvitationMessage(ulong targetId) {
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.targetId = reader.ReadVarUhLong();

            if (this.targetId < 0 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
        }
    }
}