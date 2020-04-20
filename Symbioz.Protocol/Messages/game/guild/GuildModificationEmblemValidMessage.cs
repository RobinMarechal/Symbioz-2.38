using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildModificationEmblemValidMessage : Message {
        public const ushort Id = 6328;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildEmblem guildEmblem;


        public GuildModificationEmblemValidMessage() { }

        public GuildModificationEmblemValidMessage(GuildEmblem guildEmblem) {
            this.guildEmblem = guildEmblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.guildEmblem.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildEmblem = new GuildEmblem();
            this.guildEmblem.Deserialize(reader);
        }
    }
}