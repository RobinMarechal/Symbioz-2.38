// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamMemberCompanionInformations : FightTeamMemberInformations {
        public const short Id = 451;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte companionId;
        public byte level;
        public int masterId;


        public FightTeamMemberCompanionInformations() { }

        public FightTeamMemberCompanionInformations(double id, sbyte companionId, byte level, int masterId)
            : base(id) {
            this.companionId = companionId;
            this.level = level;
            this.masterId = masterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.companionId);
            writer.WriteByte(this.level);
            writer.WriteInt(this.masterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.companionId = reader.ReadSByte();

            if (this.companionId < 0)
                throw new Exception("Forbidden value on companionId = " + this.companionId + ", it doesn't respect the following condition : companionId < 0");
            this.level = reader.ReadByte();

            if (this.level < 1 || this.level > 200)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 1 || level > 200");
            this.masterId = reader.ReadInt();
        }
    }
}