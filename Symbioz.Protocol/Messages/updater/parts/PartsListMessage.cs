using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartsListMessage : Message {
        public const ushort Id = 1502;

        public override ushort MessageId {
            get { return Id; }
        }

        public ContentPart[] parts;


        public PartsListMessage() { }

        public PartsListMessage(ContentPart[] parts) {
            this.parts = parts;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.parts.Length);
            foreach (var entry in this.parts) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.parts = new ContentPart[limit];
            for (int i = 0; i < limit; i++) {
                this.parts[i] = new ContentPart();
                this.parts[i].Deserialize(reader);
            }
        }
    }
}