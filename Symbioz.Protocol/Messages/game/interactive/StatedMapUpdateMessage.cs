using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StatedMapUpdateMessage : Message {
        public const ushort Id = 5716;

        public override ushort MessageId {
            get { return Id; }
        }

        public StatedElement[] statedElements;


        public StatedMapUpdateMessage() { }

        public StatedMapUpdateMessage(StatedElement[] statedElements) {
            this.statedElements = statedElements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.statedElements.Length);
            foreach (var entry in this.statedElements) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.statedElements = new StatedElement[limit];
            for (int i = 0; i < limit; i++) {
                this.statedElements[i] = new StatedElement();
                this.statedElements[i].Deserialize(reader);
            }
        }
    }
}