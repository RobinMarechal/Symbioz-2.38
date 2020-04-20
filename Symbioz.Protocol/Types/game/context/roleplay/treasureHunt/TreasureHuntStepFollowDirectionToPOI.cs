// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TreasureHuntStepFollowDirectionToPOI : TreasureHuntStep {
        public const short Id = 461;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte direction;
        public ushort poiLabelId;


        public TreasureHuntStepFollowDirectionToPOI() { }

        public TreasureHuntStepFollowDirectionToPOI(sbyte direction, ushort poiLabelId) {
            this.direction = direction;
            this.poiLabelId = poiLabelId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.direction);
            writer.WriteVarUhShort(this.poiLabelId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.direction = reader.ReadSByte();

            if (this.direction < 0)
                throw new Exception("Forbidden value on direction = " + this.direction + ", it doesn't respect the following condition : direction < 0");
            this.poiLabelId = reader.ReadVarUhShort();

            if (this.poiLabelId < 0)
                throw new Exception("Forbidden value on poiLabelId = " + this.poiLabelId + ", it doesn't respect the following condition : poiLabelId < 0");
        }
    }
}