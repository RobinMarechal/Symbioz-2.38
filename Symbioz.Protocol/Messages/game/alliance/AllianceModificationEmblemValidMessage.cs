using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceModificationEmblemValidMessage : Message {
        public const ushort Id = 6447;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildEmblem Alliancemblem;


        public AllianceModificationEmblemValidMessage() { }

        public AllianceModificationEmblemValidMessage(GuildEmblem Alliancemblem) {
            this.Alliancemblem = Alliancemblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.Alliancemblem.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.Alliancemblem = new GuildEmblem();
            this.Alliancemblem.Deserialize(reader);
        }
    }
}