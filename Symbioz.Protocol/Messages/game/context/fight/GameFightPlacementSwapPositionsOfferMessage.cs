using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightPlacementSwapPositionsOfferMessage : Message {
        public const ushort Id = 6542;

        public override ushort MessageId {
            get { return Id; }
        }

        public int requestId;
        public double requesterId;
        public ushort requesterCellId;
        public double requestedId;
        public ushort requestedCellId;


        public GameFightPlacementSwapPositionsOfferMessage() { }

        public GameFightPlacementSwapPositionsOfferMessage(int requestId, double requesterId, ushort requesterCellId, double requestedId, ushort requestedCellId) {
            this.requestId = requestId;
            this.requesterId = requesterId;
            this.requesterCellId = requesterCellId;
            this.requestedId = requestedId;
            this.requestedCellId = requestedCellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.requestId);
            writer.WriteDouble(this.requesterId);
            writer.WriteVarUhShort(this.requesterCellId);
            writer.WriteDouble(this.requestedId);
            writer.WriteVarUhShort(this.requestedCellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requestId = reader.ReadInt();

            if (this.requestId < 0)
                throw new Exception("Forbidden value on requestId = " + this.requestId + ", it doesn't respect the following condition : requestId < 0");
            this.requesterId = reader.ReadDouble();

            if (this.requesterId < -9007199254740990 || this.requesterId > 9007199254740990)
                throw new Exception("Forbidden value on requesterId = " + this.requesterId + ", it doesn't respect the following condition : requesterId < -9007199254740990 || requesterId > 9007199254740990");
            this.requesterCellId = reader.ReadVarUhShort();

            if (this.requesterCellId < 0 || this.requesterCellId > 559)
                throw new Exception("Forbidden value on requesterCellId = " + this.requesterCellId + ", it doesn't respect the following condition : requesterCellId < 0 || requesterCellId > 559");
            this.requestedId = reader.ReadDouble();

            if (this.requestedId < -9007199254740990 || this.requestedId > 9007199254740990)
                throw new Exception("Forbidden value on requestedId = " + this.requestedId + ", it doesn't respect the following condition : requestedId < -9007199254740990 || requestedId > 9007199254740990");
            this.requestedCellId = reader.ReadVarUhShort();

            if (this.requestedCellId < 0 || this.requestedCellId > 559)
                throw new Exception("Forbidden value on requestedCellId = " + this.requestedCellId + ", it doesn't respect the following condition : requestedCellId < 0 || requestedCellId > 559");
        }
    }
}