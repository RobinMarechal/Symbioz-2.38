// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class AlliancePrismInformation : PrismInformation {
        public const short Id = 427;

        public override short TypeId {
            get { return Id; }
        }

        public AllianceInformations alliance;


        public AlliancePrismInformation() { }

        public AlliancePrismInformation(sbyte typeId, sbyte state, int nextVulnerabilityDate, int placementDate, uint rewardTokenCount, AllianceInformations alliance)
            : base(typeId, state, nextVulnerabilityDate, placementDate, rewardTokenCount) {
            this.alliance = alliance;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.alliance.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.alliance = new AllianceInformations();
            this.alliance.Deserialize(reader);
        }
    }
}