// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamMemberMonsterInformations : FightTeamMemberInformations {
        public const short Id = 6;

        public override short TypeId {
            get { return Id; }
        }

        public int monsterId;
        public sbyte grade;


        public FightTeamMemberMonsterInformations() { }

        public FightTeamMemberMonsterInformations(double id, int monsterId, sbyte grade)
            : base(id) {
            this.monsterId = monsterId;
            this.grade = grade;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.monsterId);
            writer.WriteSByte(this.grade);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.monsterId = reader.ReadInt();
            this.grade = reader.ReadSByte();

            if (this.grade < 0)
                throw new Exception("Forbidden value on grade = " + this.grade + ", it doesn't respect the following condition : grade < 0");
        }
    }
}