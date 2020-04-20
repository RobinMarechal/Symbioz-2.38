using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceChangeGuildRightsMessage : Message {
        public const ushort Id = 6426;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint guildId;
        public sbyte rights;


        public AllianceChangeGuildRightsMessage() { }

        public AllianceChangeGuildRightsMessage(uint guildId, sbyte rights) {
            this.guildId = guildId;
            this.rights = rights;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.guildId);
            writer.WriteSByte(this.rights);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildId = reader.ReadVarUhInt();

            if (this.guildId < 0)
                throw new Exception("Forbidden value on guildId = " + this.guildId + ", it doesn't respect the following condition : guildId < 0");
            this.rights = reader.ReadSByte();

            if (this.rights < 0)
                throw new Exception("Forbidden value on rights = " + this.rights + ", it doesn't respect the following condition : rights < 0");
        }
    }
}