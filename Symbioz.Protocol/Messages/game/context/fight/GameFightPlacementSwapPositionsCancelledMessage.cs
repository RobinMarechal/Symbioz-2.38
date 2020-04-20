using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightPlacementSwapPositionsCancelledMessage : Message {
        public const ushort Id = 6546;

        public override ushort MessageId {
            get { return Id; }
        }

        public int requestId;
        public double cancellerId;


        public GameFightPlacementSwapPositionsCancelledMessage() { }

        public GameFightPlacementSwapPositionsCancelledMessage(int requestId, double cancellerId) {
            this.requestId = requestId;
            this.cancellerId = cancellerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.requestId);
            writer.WriteDouble(this.cancellerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requestId = reader.ReadInt();

            if (this.requestId < 0)
                throw new Exception("Forbidden value on requestId = " + this.requestId + ", it doesn't respect the following condition : requestId < 0");
            this.cancellerId = reader.ReadDouble();

            if (this.cancellerId < -9007199254740990 || this.cancellerId > 9007199254740990)
                throw new Exception("Forbidden value on cancellerId = " + this.cancellerId + ", it doesn't respect the following condition : cancellerId < -9007199254740990 || cancellerId > 9007199254740990");
        }
    }
}