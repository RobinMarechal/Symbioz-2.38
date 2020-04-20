using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightReduceDamagesMessage : AbstractGameActionMessage {
        public const ushort Id = 5526;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public uint amount;


        public GameActionFightReduceDamagesMessage() { }

        public GameActionFightReduceDamagesMessage(ushort actionId, double sourceId, double targetId, uint amount)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.amount = amount;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhInt(this.amount);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.amount = reader.ReadVarUhInt();

            if (this.amount < 0)
                throw new Exception("Forbidden value on amount = " + this.amount + ", it doesn't respect the following condition : amount < 0");
        }
    }
}