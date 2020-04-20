using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightThrowCharacterMessage : AbstractGameActionMessage {
        public const ushort Id = 5829;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public short cellId;


        public GameActionFightThrowCharacterMessage() { }

        public GameActionFightThrowCharacterMessage(ushort actionId, double sourceId, double targetId, short cellId)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.cellId = cellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteShort(this.cellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.cellId = reader.ReadShort();

            if (this.cellId < -1 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
        }
    }
}