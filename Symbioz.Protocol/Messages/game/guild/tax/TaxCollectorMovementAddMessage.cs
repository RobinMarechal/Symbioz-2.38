using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorMovementAddMessage : Message {
        public const ushort Id = 5917;

        public override ushort MessageId {
            get { return Id; }
        }

        public TaxCollectorInformations informations;


        public TaxCollectorMovementAddMessage() { }

        public TaxCollectorMovementAddMessage(TaxCollectorInformations informations) {
            this.informations = informations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.informations.TypeId);
            this.informations.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.informations = ProtocolTypeManager.GetInstance<TaxCollectorInformations>(reader.ReadShort());
            this.informations.Deserialize(reader);
        }
    }
}