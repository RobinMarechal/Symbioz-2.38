using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildJoinedMessage : Message {
        public const ushort Id = 5564;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildInformations guildInfo;
        public uint memberRights;
        public bool enabled;


        public GuildJoinedMessage() { }

        public GuildJoinedMessage(GuildInformations guildInfo, uint memberRights, bool enabled) {
            this.guildInfo = guildInfo;
            this.memberRights = memberRights;
            this.enabled = enabled;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.guildInfo.Serialize(writer);
            writer.WriteVarUhInt(this.memberRights);
            writer.WriteBoolean(this.enabled);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildInfo = new GuildInformations();
            this.guildInfo.Deserialize(reader);
            this.memberRights = reader.ReadVarUhInt();

            if (this.memberRights < 0)
                throw new Exception("Forbidden value on memberRights = " + this.memberRights + ", it doesn't respect the following condition : memberRights < 0");
            this.enabled = reader.ReadBoolean();
        }
    }
}