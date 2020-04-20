using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AtlasPointInformationsMessage : Message {
        public const ushort Id = 5956;

        public override ushort MessageId {
            get { return Id; }
        }

        public AtlasPointsInformations type;


        public AtlasPointInformationsMessage() { }

        public AtlasPointInformationsMessage(AtlasPointsInformations type) {
            this.type = type;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.type.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.type = new AtlasPointsInformations();
            this.type.Deserialize(reader);
        }
    }
}