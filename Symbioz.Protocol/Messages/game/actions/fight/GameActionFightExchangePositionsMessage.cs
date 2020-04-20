using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightExchangePositionsMessage : AbstractGameActionMessage {
        public const ushort Id = 5527;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public short casterCellId;
        public short targetCellId;


        public GameActionFightExchangePositionsMessage() { }

        public GameActionFightExchangePositionsMessage(ushort actionId, double sourceId, double targetId, short casterCellId, short targetCellId)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.casterCellId = casterCellId;
            this.targetCellId = targetCellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteShort(this.casterCellId);
            writer.WriteShort(this.targetCellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.casterCellId = reader.ReadShort();

            if (this.casterCellId < -1 || this.casterCellId > 559)
                throw new Exception("Forbidden value on casterCellId = " + this.casterCellId + ", it doesn't respect the following condition : casterCellId < -1 || casterCellId > 559");
            this.targetCellId = reader.ReadShort();

            if (this.targetCellId < -1 || this.targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + this.targetCellId + ", it doesn't respect the following condition : targetCellId < -1 || targetCellId > 559");
        }
    }
}