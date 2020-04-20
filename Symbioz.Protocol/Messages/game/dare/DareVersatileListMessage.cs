using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareVersatileListMessage : Message {
        public const ushort Id = 6657;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareVersatileInformations[] dares;


        public DareVersatileListMessage() { }

        public DareVersatileListMessage(DareVersatileInformations[] dares) {
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
            this.dares = new DareVersatileInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.dares[i] = new DareVersatileInformations();
                this.dares[i].Deserialize(reader);
            }
        }
    }
}