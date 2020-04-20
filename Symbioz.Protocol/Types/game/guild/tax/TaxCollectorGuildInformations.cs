// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TaxCollectorGuildInformations : TaxCollectorComplementaryInformations {
        public const short Id = 446;

        public override short TypeId {
            get { return Id; }
        }

        public BasicGuildInformations guild;


        public TaxCollectorGuildInformations() { }

        public TaxCollectorGuildInformations(BasicGuildInformations guild) {
            this.guild = guild;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.guild.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.guild = new BasicGuildInformations();
            this.guild.Deserialize(reader);
        }
    }
}