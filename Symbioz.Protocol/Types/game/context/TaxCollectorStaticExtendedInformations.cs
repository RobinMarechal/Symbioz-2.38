// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TaxCollectorStaticExtendedInformations : TaxCollectorStaticInformations {
        public const short Id = 440;

        public override short TypeId {
            get { return Id; }
        }

        public AllianceInformations allianceIdentity;


        public TaxCollectorStaticExtendedInformations() { }

        public TaxCollectorStaticExtendedInformations(ushort firstNameId, ushort lastNameId, GuildInformations guildIdentity, AllianceInformations allianceIdentity)
            : base(firstNameId, lastNameId, guildIdentity) {
            this.allianceIdentity = allianceIdentity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.allianceIdentity.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.allianceIdentity = new AllianceInformations();
            this.allianceIdentity.Deserialize(reader);
        }
    }
}