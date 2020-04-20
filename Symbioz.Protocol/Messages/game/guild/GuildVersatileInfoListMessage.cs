using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildVersatileInfoListMessage : Message {
        public const ushort Id = 6435;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildVersatileInformations[] guilds;


        public GuildVersatileInfoListMessage() { }

        public GuildVersatileInfoListMessage(GuildVersatileInformations[] guilds) {
            this.guilds = guilds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.guilds.Length);
            foreach (var entry in this.guilds) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.guilds = new GuildVersatileInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guilds[i] = ProtocolTypeManager.GetInstance<GuildVersatileInformations>(reader.ReadShort());
                this.guilds[i].Deserialize(reader);
            }
        }
    }
}