// Generated on 04/27/2016 01:13:08

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class AchievementObjective {
        public const short Id = 404;

        public virtual short TypeId {
            get { return Id; }
        }

        public uint id;
        public ushort maxValue;


        public AchievementObjective() { }

        public AchievementObjective(uint id, ushort maxValue) {
            this.id = id;
            this.maxValue = maxValue;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.id);
            writer.WriteVarUhShort(this.maxValue);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadVarUhInt();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
            this.maxValue = reader.ReadVarUhShort();

            if (this.maxValue < 0)
                throw new Exception("Forbidden value on maxValue = " + this.maxValue + ", it doesn't respect the following condition : maxValue < 0");
        }
    }
}