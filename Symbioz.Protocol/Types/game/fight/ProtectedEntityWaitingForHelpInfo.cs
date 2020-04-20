// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ProtectedEntityWaitingForHelpInfo {
        public const short Id = 186;

        public virtual short TypeId {
            get { return Id; }
        }

        public int timeLeftBeforeFight;
        public int waitTimeForPlacement;
        public sbyte nbPositionForDefensors;


        public ProtectedEntityWaitingForHelpInfo() { }

        public ProtectedEntityWaitingForHelpInfo(int timeLeftBeforeFight, int waitTimeForPlacement, sbyte nbPositionForDefensors) {
            this.timeLeftBeforeFight = timeLeftBeforeFight;
            this.waitTimeForPlacement = waitTimeForPlacement;
            this.nbPositionForDefensors = nbPositionForDefensors;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.timeLeftBeforeFight);
            writer.WriteInt(this.waitTimeForPlacement);
            writer.WriteSByte(this.nbPositionForDefensors);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.timeLeftBeforeFight = reader.ReadInt();
            this.waitTimeForPlacement = reader.ReadInt();
            this.nbPositionForDefensors = reader.ReadSByte();

            if (this.nbPositionForDefensors < 0)
                throw new Exception("Forbidden value on nbPositionForDefensors = " + this.nbPositionForDefensors + ", it doesn't respect the following condition : nbPositionForDefensors < 0");
        }
    }
}