using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AccessoryPreviewRequestMessage : Message {
        public const ushort Id = 6518;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] genericId;


        public AccessoryPreviewRequestMessage() { }

        public AccessoryPreviewRequestMessage(ushort[] genericId) {
            this.genericId = genericId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.genericId.Length);
            foreach (var entry in this.genericId) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.genericId = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.genericId[i] = reader.ReadVarUhShort();
            }
        }
    }
}