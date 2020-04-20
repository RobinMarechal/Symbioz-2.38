// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HumanOptionObjectUse : HumanOption {
        public const short Id = 449;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte delayTypeId;
        public double delayEndTime;
        public ushort objectGID;


        public HumanOptionObjectUse() { }

        public HumanOptionObjectUse(sbyte delayTypeId, double delayEndTime, ushort objectGID) {
            this.delayTypeId = delayTypeId;
            this.delayEndTime = delayEndTime;
            this.objectGID = objectGID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.delayTypeId);
            writer.WriteDouble(this.delayEndTime);
            writer.WriteVarUhShort(this.objectGID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.delayTypeId = reader.ReadSByte();

            if (this.delayTypeId < 0)
                throw new Exception("Forbidden value on delayTypeId = " + this.delayTypeId + ", it doesn't respect the following condition : delayTypeId < 0");
            this.delayEndTime = reader.ReadDouble();

            if (this.delayEndTime < 0 || this.delayEndTime > 9007199254740990)
                throw new Exception("Forbidden value on delayEndTime = " + this.delayEndTime + ", it doesn't respect the following condition : delayEndTime < 0 || delayEndTime > 9007199254740990");
            this.objectGID = reader.ReadVarUhShort();

            if (this.objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + this.objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
    }
}