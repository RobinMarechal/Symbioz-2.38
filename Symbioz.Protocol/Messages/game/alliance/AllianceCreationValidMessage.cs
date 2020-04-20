using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceCreationValidMessage : Message {
        public const ushort Id = 6393;

        public override ushort MessageId {
            get { return Id; }
        }

        public string allianceName;
        public string allianceTag;
        public GuildEmblem allianceEmblem;


        public AllianceCreationValidMessage() { }

        public AllianceCreationValidMessage(string allianceName, string allianceTag, GuildEmblem allianceEmblem) {
            this.allianceName = allianceName;
            this.allianceTag = allianceTag;
            this.allianceEmblem = allianceEmblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.allianceName);
            writer.WriteUTF(this.allianceTag);
            this.allianceEmblem.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceName = reader.ReadUTF();
            this.allianceTag = reader.ReadUTF();
            this.allianceEmblem = new GuildEmblem();
            this.allianceEmblem.Deserialize(reader);
        }
    }
}