// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HouseInformationsExtended : HouseInformations {
        public const short Id = 112;

        public override short TypeId {
            get { return Id; }
        }

        public GuildInformations guildInfo;


        public HouseInformationsExtended() { }

        public HouseInformationsExtended(uint houseId, int[] doorsOnMap, string ownerName, ushort modelId, GuildInformations guildInfo)
            : base(houseId, doorsOnMap, ownerName, modelId) {
            this.guildInfo = guildInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.guildInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.guildInfo = new GuildInformations();
            this.guildInfo.Deserialize(reader);
        }
    }
}