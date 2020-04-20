using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartInfoMessage : Message {
        public const ushort Id = 1508;

        public override ushort MessageId {
            get { return Id; }
        }

        public ContentPart part;
        public float installationPercent;


        public PartInfoMessage() { }

        public PartInfoMessage(ContentPart part, float installationPercent) {
            this.part = part;
            this.installationPercent = installationPercent;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.part.Serialize(writer);
            writer.WriteFloat(this.installationPercent);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.part = new ContentPart();
            this.part.Deserialize(reader);
            this.installationPercent = reader.ReadFloat();
        }
    }
}