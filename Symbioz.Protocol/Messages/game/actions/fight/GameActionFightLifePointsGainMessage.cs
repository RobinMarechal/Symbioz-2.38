using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightLifePointsGainMessage : AbstractGameActionMessage {
        public const ushort Id = 6311;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public uint delta;


        public GameActionFightLifePointsGainMessage() { }

        public GameActionFightLifePointsGainMessage(ushort actionId, double sourceId, double targetId, uint delta)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.delta = delta;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhInt(this.delta);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.delta = reader.ReadVarUhInt();

            if (this.delta < 0)
                throw new Exception("Forbidden value on delta = " + this.delta + ", it doesn't respect the following condition : delta < 0");
        }
    }
}