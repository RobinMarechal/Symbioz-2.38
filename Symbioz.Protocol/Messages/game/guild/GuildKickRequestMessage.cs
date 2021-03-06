using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildKickRequestMessage : Message {
        public const ushort Id = 5887;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong kickedId;


        public GuildKickRequestMessage() { }

        public GuildKickRequestMessage(ulong kickedId) {
            this.kickedId = kickedId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.kickedId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.kickedId = reader.ReadVarUhLong();

            if (this.kickedId < 0 || this.kickedId > 9007199254740990)
                throw new Exception("Forbidden value on kickedId = " + this.kickedId + ", it doesn't respect the following condition : kickedId < 0 || kickedId > 9007199254740990");
        }
    }
}