// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TreasureHuntStepFollowDirection : TreasureHuntStep {
        public const short Id = 468;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte direction;
        public ushort mapCount;


        public TreasureHuntStepFollowDirection() { }

        public TreasureHuntStepFollowDirection(sbyte direction, ushort mapCount) {
            this.direction = direction;
            this.mapCount = mapCount;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.direction);
            writer.WriteVarUhShort(this.mapCount);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.direction = reader.ReadSByte();

            if (this.direction < 0)
                throw new Exception("Forbidden value on direction = " + this.direction + ", it doesn't respect the following condition : direction < 0");
            this.mapCount = reader.ReadVarUhShort();

            if (this.mapCount < 0)
                throw new Exception("Forbidden value on mapCount = " + this.mapCount + ", it doesn't respect the following condition : mapCount < 0");
        }
    }
}