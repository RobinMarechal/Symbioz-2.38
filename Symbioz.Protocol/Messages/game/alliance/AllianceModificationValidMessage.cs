using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceModificationValidMessage : Message {
        public const ushort Id = 6450;

        public override ushort MessageId {
            get { return Id; }
        }

        public string allianceName;
        public string allianceTag;
        public GuildEmblem Alliancemblem;


        public AllianceModificationValidMessage() { }

        public AllianceModificationValidMessage(string allianceName, string allianceTag, GuildEmblem Alliancemblem) {
            this.allianceName = allianceName;
            this.allianceTag = allianceTag;
            this.Alliancemblem = Alliancemblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.allianceName);
            writer.WriteUTF(this.allianceTag);
            this.Alliancemblem.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceName = reader.ReadUTF();
            this.allianceTag = reader.ReadUTF();
            this.Alliancemblem = new GuildEmblem();
            this.Alliancemblem.Deserialize(reader);
        }
    }
}