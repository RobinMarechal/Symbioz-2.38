using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AbstractTaxCollectorListMessage : Message {
        public const ushort Id = 6568;

        public override ushort MessageId {
            get { return Id; }
        }

        public TaxCollectorInformations[] informations;


        public AbstractTaxCollectorListMessage() { }

        public AbstractTaxCollectorListMessage(TaxCollectorInformations[] informations) {
            this.informations = informations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.informations.Length);
            foreach (var entry in this.informations) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.informations = new TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.informations[i] = ProtocolTypeManager.GetInstance<TaxCollectorInformations>(reader.ReadShort());
                this.informations[i].Deserialize(reader);
            }
        }
    }
}