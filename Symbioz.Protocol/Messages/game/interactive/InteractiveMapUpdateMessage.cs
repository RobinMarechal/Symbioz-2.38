using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InteractiveMapUpdateMessage : Message {
        public const ushort Id = 5002;

        public override ushort MessageId {
            get { return Id; }
        }

        public InteractiveElement[] interactiveElements;


        public InteractiveMapUpdateMessage() { }

        public InteractiveMapUpdateMessage(InteractiveElement[] interactiveElements) {
            this.interactiveElements = interactiveElements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.interactiveElements.Length);
            foreach (var entry in this.interactiveElements) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.interactiveElements = new InteractiveElement[limit];
            for (int i = 0; i < limit; i++) {
                this.interactiveElements[i] = ProtocolTypeManager.GetInstance<InteractiveElement>(reader.ReadShort());
                this.interactiveElements[i].Deserialize(reader);
            }
        }
    }
}