// Generated on 04/27/2016 01:13:19

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GuildInAllianceVersatileInformations : GuildVersatileInformations {
        public const short Id = 437;

        public override short TypeId {
            get { return Id; }
        }

        public uint allianceId;


        public GuildInAllianceVersatileInformations() { }

        public GuildInAllianceVersatileInformations(uint guildId, ulong leaderId, byte guildLevel, byte nbMembers, uint allianceId)
            : base(guildId, leaderId, guildLevel, nbMembers) {
            this.allianceId = allianceId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.allianceId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.allianceId = reader.ReadVarUhInt();

            if (this.allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + this.allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
    }
}