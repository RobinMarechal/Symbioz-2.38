using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismsListMessage : Message {
        public const ushort Id = 6440;

        public override ushort MessageId {
            get { return Id; }
        }

        public PrismSubareaEmptyInfo[] prisms;


        public PrismsListMessage() { }

        public PrismsListMessage(PrismSubareaEmptyInfo[] prisms) {
            this.prisms = prisms;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.prisms.Length);
            foreach (var entry in this.prisms) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.prisms = new PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.prisms[i] = ProtocolTypeManager.GetInstance<PrismSubareaEmptyInfo>(reader.ReadShort());
                this.prisms[i].Deserialize(reader);
            }
        }
    }
}