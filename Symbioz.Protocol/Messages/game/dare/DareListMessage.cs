using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareListMessage : Message {
        public const ushort Id = 6661;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareInformations[] dares;


        public DareListMessage() { }

        public DareListMessage(DareInformations[] dares) {
            this.dares = dares;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.dares.Length);
            foreach (var entry in this.dares) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.dares = new DareInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.dares[i] = new DareInformations();
                this.dares[i].Deserialize(reader);
            }
        }
    }
}