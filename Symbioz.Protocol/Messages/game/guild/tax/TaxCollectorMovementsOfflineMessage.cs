using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorMovementsOfflineMessage : Message {
        public const ushort Id = 6611;

        public override ushort MessageId {
            get { return Id; }
        }

        public TaxCollectorMovement[] movements;


        public TaxCollectorMovementsOfflineMessage() { }

        public TaxCollectorMovementsOfflineMessage(TaxCollectorMovement[] movements) {
            this.movements = movements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.movements.Length);
            foreach (var entry in this.movements) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.movements = new TaxCollectorMovement[limit];
            for (int i = 0; i < limit; i++) {
                this.movements[i] = new TaxCollectorMovement();
                this.movements[i].Deserialize(reader);
            }
        }
    }
}