using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightInvisibilityMessage : AbstractGameActionMessage {
        public const ushort Id = 5821;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public sbyte state;


        public GameActionFightInvisibilityMessage() { }

        public GameActionFightInvisibilityMessage(ushort actionId, double sourceId, double targetId, sbyte state)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.state = state;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteSByte(this.state);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.state = reader.ReadSByte();

            if (this.state < 0)
                throw new Exception("Forbidden value on state = " + this.state + ", it doesn't respect the following condition : state < 0");
        }
    }
}