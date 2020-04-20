using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInformationsMembersMessage : Message {
        public const ushort Id = 5558;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildMember[] members;


        public GuildInformationsMembersMessage() { }

        public GuildInformationsMembersMessage(GuildMember[] members) {
            this.members = members;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.members.Length);
            foreach (var entry in this.members) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.members = new GuildMember[limit];
            for (int i = 0; i < limit; i++) {
                this.members[i] = new GuildMember();
                this.members[i].Deserialize(reader);
            }
        }
    }
}