// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightResultPvpData : FightResultAdditionalData {
        public const short Id = 190;

        public override short TypeId {
            get { return Id; }
        }

        public byte grade;
        public ushort minHonorForGrade;
        public ushort maxHonorForGrade;
        public ushort honor;
        public short honorDelta;


        public FightResultPvpData() { }

        public FightResultPvpData(byte grade, ushort minHonorForGrade, ushort maxHonorForGrade, ushort honor, short honorDelta) {
            this.grade = grade;
            this.minHonorForGrade = minHonorForGrade;
            this.maxHonorForGrade = maxHonorForGrade;
            this.honor = honor;
            this.honorDelta = honorDelta;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteByte(this.grade);
            writer.WriteVarUhShort(this.minHonorForGrade);
            writer.WriteVarUhShort(this.maxHonorForGrade);
            writer.WriteVarUhShort(this.honor);
            writer.WriteVarShort(this.honorDelta);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.grade = reader.ReadByte();

            if (this.grade < 0 || this.grade > 255)
                throw new Exception("Forbidden value on grade = " + this.grade + ", it doesn't respect the following condition : grade < 0 || grade > 255");
            this.minHonorForGrade = reader.ReadVarUhShort();

            if (this.minHonorForGrade < 0 || this.minHonorForGrade > 20000)
                throw new Exception("Forbidden value on minHonorForGrade = " + this.minHonorForGrade + ", it doesn't respect the following condition : minHonorForGrade < 0 || minHonorForGrade > 20000");
            this.maxHonorForGrade = reader.ReadVarUhShort();

            if (this.maxHonorForGrade < 0 || this.maxHonorForGrade > 20000)
                throw new Exception("Forbidden value on maxHonorForGrade = " + this.maxHonorForGrade + ", it doesn't respect the following condition : maxHonorForGrade < 0 || maxHonorForGrade > 20000");
            this.honor = reader.ReadVarUhShort();

            if (this.honor < 0 || this.honor > 20000)
                throw new Exception("Forbidden value on honor = " + this.honor + ", it doesn't respect the following condition : honor < 0 || honor > 20000");
            this.honorDelta = reader.ReadVarShort();
        }
    }
}