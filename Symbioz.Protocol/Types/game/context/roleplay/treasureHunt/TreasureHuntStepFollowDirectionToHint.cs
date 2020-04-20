// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TreasureHuntStepFollowDirectionToHint : TreasureHuntStep {
        public const short Id = 472;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte direction;
        public ushort npcId;


        public TreasureHuntStepFollowDirectionToHint() { }

        public TreasureHuntStepFollowDirectionToHint(sbyte direction, ushort npcId) {
            this.direction = direction;
            this.npcId = npcId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.direction);
            writer.WriteVarUhShort(this.npcId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.direction = reader.ReadSByte();

            if (this.direction < 0)
                throw new Exception("Forbidden value on direction = " + this.direction + ", it doesn't respect the following condition : direction < 0");
            this.npcId = reader.ReadVarUhShort();

            if (this.npcId < 0)
                throw new Exception("Forbidden value on npcId = " + this.npcId + ", it doesn't respect the following condition : npcId < 0");
        }
    }
}