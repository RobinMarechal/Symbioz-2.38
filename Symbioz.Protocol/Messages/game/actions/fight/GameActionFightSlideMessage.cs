using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightSlideMessage : AbstractGameActionMessage {
        public const ushort Id = 5525;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public short startCellId;
        public short endCellId;


        public GameActionFightSlideMessage() { }

        public GameActionFightSlideMessage(ushort actionId, double sourceId, double targetId, short startCellId, short endCellId)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.startCellId = startCellId;
            this.endCellId = endCellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteShort(this.startCellId);
            writer.WriteShort(this.endCellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.startCellId = reader.ReadShort();

            if (this.startCellId < -1 || this.startCellId > 559)
                throw new Exception("Forbidden value on startCellId = " + this.startCellId + ", it doesn't respect the following condition : startCellId < -1 || startCellId > 559");
            this.endCellId = reader.ReadShort();

            if (this.endCellId < -1 || this.endCellId > 559)
                throw new Exception("Forbidden value on endCellId = " + this.endCellId + ", it doesn't respect the following condition : endCellId < -1 || endCellId > 559");
        }
    }
}