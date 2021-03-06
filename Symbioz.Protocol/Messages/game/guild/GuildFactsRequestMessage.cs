using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFactsRequestMessage : Message {
        public const ushort Id = 6404;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint guildId;


        public GuildFactsRequestMessage() { }

        public GuildFactsRequestMessage(uint guildId) {
            this.guildId = guildId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.guildId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildId = reader.ReadVarUhInt();

            if (this.guildId < 0)
                throw new Exception("Forbidden value on guildId = " + this.guildId + ", it doesn't respect the following condition : guildId < 0");
        }
    }
}