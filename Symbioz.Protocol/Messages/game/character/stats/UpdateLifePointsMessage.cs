using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class UpdateLifePointsMessage : Message {
        public const ushort Id = 5658;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint lifePoints;
        public uint maxLifePoints;


        public UpdateLifePointsMessage() { }

        public UpdateLifePointsMessage(uint lifePoints, uint maxLifePoints) {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.lifePoints);
            writer.WriteVarUhInt(this.maxLifePoints);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.lifePoints = reader.ReadVarUhInt();

            if (this.lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + this.lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            this.maxLifePoints = reader.ReadVarUhInt();

            if (this.maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + this.maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
        }
    }
}