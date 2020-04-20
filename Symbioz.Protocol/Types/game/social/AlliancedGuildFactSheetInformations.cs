// Generated on 04/27/2016 01:13:19

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class AlliancedGuildFactSheetInformations : GuildInformations {
        public const short Id = 422;

        public override short TypeId {
            get { return Id; }
        }

        public BasicNamedAllianceInformations allianceInfos;


        public AlliancedGuildFactSheetInformations() { }

        public AlliancedGuildFactSheetInformations(uint guildId, string guildName, byte guildLevel, GuildEmblem guildEmblem, BasicNamedAllianceInformations allianceInfos)
            : base(guildId, guildName, guildLevel, guildEmblem) {
            this.allianceInfos = allianceInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.allianceInfos.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.allianceInfos = new BasicNamedAllianceInformations();
            this.allianceInfos.Deserialize(reader);
        }
    }
}