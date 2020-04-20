using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseGuildRightsMessage : Message {
        public const ushort Id = 5703;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint houseId;
        public GuildInformations guildInfo;
        public uint rights;


        public HouseGuildRightsMessage() { }

        public HouseGuildRightsMessage(uint houseId, GuildInformations guildInfo, uint rights) {
            this.houseId = houseId;
            this.guildInfo = guildInfo;
            this.rights = rights;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.houseId);
            this.guildInfo.Serialize(writer);
            writer.WriteVarUhInt(this.rights);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.houseId = reader.ReadVarUhInt();

            if (this.houseId < 0)
                throw new Exception("Forbidden value on houseId = " + this.houseId + ", it doesn't respect the following condition : houseId < 0");
            this.guildInfo = new GuildInformations();
            this.guildInfo.Deserialize(reader);
            this.rights = reader.ReadVarUhInt();

            if (this.rights < 0)
                throw new Exception("Forbidden value on rights = " + this.rights + ", it doesn't respect the following condition : rights < 0");
        }
    }
}