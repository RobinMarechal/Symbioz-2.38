using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildModificationValidMessage : Message {
        public const ushort Id = 6323;

        public override ushort MessageId {
            get { return Id; }
        }

        public string guildName;
        public GuildEmblem guildEmblem;


        public GuildModificationValidMessage() { }

        public GuildModificationValidMessage(string guildName, GuildEmblem guildEmblem) {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.guildName);
            this.guildEmblem.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildName = reader.ReadUTF();
            this.guildEmblem = new GuildEmblem();
            this.guildEmblem.Deserialize(reader);
        }
    }
}