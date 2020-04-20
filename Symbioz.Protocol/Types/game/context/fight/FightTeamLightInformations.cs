// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamLightInformations : AbstractFightTeamInformations {
        public const short Id = 115;
        public override short TypeId => Id;

        public sbyte teamMembersCount;
        public uint meanLevel;


        public FightTeamLightInformations() { }

        public FightTeamLightInformations(sbyte teamId, double leaderId, sbyte teamSide, sbyte teamTypeId, sbyte nbWaves, sbyte teamMembersCount, uint meanLevel)
            : base(teamId, leaderId, teamSide, teamTypeId, nbWaves) {
            this.teamMembersCount = teamMembersCount;
            this.meanLevel = meanLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.teamMembersCount);
            writer.WriteVarUhInt(this.meanLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.teamMembersCount = reader.ReadSByte();

            if (this.teamMembersCount < 0)
                throw new Exception("Forbidden value on teamMembersCount = " + this.teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
            this.meanLevel = reader.ReadVarUhInt();

            if (this.meanLevel < 0)
                throw new Exception("Forbidden value on meanLevel = " + this.meanLevel + ", it doesn't respect the following condition : meanLevel < 0");
        }
    }
}