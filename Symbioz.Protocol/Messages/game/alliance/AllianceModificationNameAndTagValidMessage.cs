using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceModificationNameAndTagValidMessage : Message {
        public const ushort Id = 6449;

        public override ushort MessageId {
            get { return Id; }
        }

        public string allianceName;
        public string allianceTag;


        public AllianceModificationNameAndTagValidMessage() { }

        public AllianceModificationNameAndTagValidMessage(string allianceName, string allianceTag) {
            this.allianceName = allianceName;
            this.allianceTag = allianceTag;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.allianceName);
            writer.WriteUTF(this.allianceTag);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceName = reader.ReadUTF();
            this.allianceTag = reader.ReadUTF();
        }
    }
}