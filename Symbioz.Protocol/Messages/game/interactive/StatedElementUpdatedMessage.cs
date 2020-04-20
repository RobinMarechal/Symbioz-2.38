using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StatedElementUpdatedMessage : Message {
        public const ushort Id = 5709;

        public override ushort MessageId {
            get { return Id; }
        }

        public StatedElement statedElement;


        public StatedElementUpdatedMessage() { }

        public StatedElementUpdatedMessage(StatedElement statedElement) {
            this.statedElement = statedElement;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.statedElement.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.statedElement = new StatedElement();
            this.statedElement.Deserialize(reader);
        }
    }
}