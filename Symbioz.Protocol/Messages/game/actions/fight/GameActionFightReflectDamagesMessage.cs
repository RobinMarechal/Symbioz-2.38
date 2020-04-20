using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightReflectDamagesMessage : AbstractGameActionMessage {
        public const ushort Id = 5530;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;


        public GameActionFightReflectDamagesMessage() { }

        public GameActionFightReflectDamagesMessage(ushort actionId, double sourceId, double targetId)
            : base(actionId, sourceId) {
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
        }
    }
}