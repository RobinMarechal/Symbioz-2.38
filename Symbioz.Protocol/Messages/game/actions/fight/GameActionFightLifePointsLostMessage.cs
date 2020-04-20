using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightLifePointsLostMessage : AbstractGameActionMessage {
        public const ushort Id = 6312;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public uint loss;
        public uint permanentDamages;


        public GameActionFightLifePointsLostMessage() { }

        public GameActionFightLifePointsLostMessage(ushort actionId, double sourceId, double targetId, uint loss, uint permanentDamages)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.loss = loss;
            this.permanentDamages = permanentDamages;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhInt(this.loss);
            writer.WriteVarUhInt(this.permanentDamages);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.loss = reader.ReadVarUhInt();

            if (this.loss < 0)
                throw new Exception("Forbidden value on loss = " + this.loss + ", it doesn't respect the following condition : loss < 0");
            this.permanentDamages = reader.ReadVarUhInt();

            if (this.permanentDamages < 0)
                throw new Exception("Forbidden value on permanentDamages = " + this.permanentDamages + ", it doesn't respect the following condition : permanentDamages < 0");
        }
    }
}