using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildListMessage : Message {
        public const ushort Id = 6413;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildInformations[] guilds;


        public GuildListMessage() { }

        public GuildListMessage(GuildInformations[] guilds) {
            this.guilds = guilds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.guilds.Length);
            foreach (var entry in this.guilds) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.guilds = new GuildInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guilds[i] = new GuildInformations();
                this.guilds[i].Deserialize(reader);
            }
        }
    }
}