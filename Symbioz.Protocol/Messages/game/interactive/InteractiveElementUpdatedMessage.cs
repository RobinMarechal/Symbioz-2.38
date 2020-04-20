using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InteractiveElementUpdatedMessage : Message {
        public const ushort Id = 5708;

        public override ushort MessageId {
            get { return Id; }
        }

        public InteractiveElement interactiveElement;


        public InteractiveElementUpdatedMessage() { }

        public InteractiveElementUpdatedMessage(InteractiveElement interactiveElement) {
            this.interactiveElement = interactiveElement;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.interactiveElement.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.interactiveElement = new InteractiveElement();
            this.interactiveElement.Deserialize(reader);
        }
    }
}