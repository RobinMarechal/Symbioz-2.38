using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorListMessage : AbstractTaxCollectorListMessage {
        public const ushort Id = 5930;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte nbcollectorMax;
        public TaxCollectorFightersInformation[] fightersInformations;


        public TaxCollectorListMessage() { }

        public TaxCollectorListMessage(TaxCollectorInformations[] informations, sbyte nbcollectorMax, TaxCollectorFightersInformation[] fightersInformations)
            : base(informations) {
            this.nbcollectorMax = nbcollectorMax;
            this.fightersInformations = fightersInformations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.nbcollectorMax);
            writer.WriteUShort((ushort) this.fightersInformations.Length);
            foreach (var entry in this.fightersInformations) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.nbcollectorMax = reader.ReadSByte();

            if (this.nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + this.nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            var limit = reader.ReadUShort();
            this.fightersInformations = new TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++) {
                this.fightersInformations[i] = new TaxCollectorFightersInformation();
                this.fightersInformations[i].Deserialize(reader);
            }
        }
    }
}