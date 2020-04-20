using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInformationsMemberUpdateMessage : Message {
        public const ushort Id = 5597;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildMember member;


        public GuildInformationsMemberUpdateMessage() { }

        public GuildInformationsMemberUpdateMessage(GuildMember member) {
            this.member = member;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.member.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.member = new GuildMember();
            this.member.Deserialize(reader);
        }
    }
}