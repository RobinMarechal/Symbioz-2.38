using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightPlacementSwapPositionsAcceptMessage : Message {
        public const ushort Id = 6547;

        public override ushort MessageId {
            get { return Id; }
        }

        public int requestId;


        public GameFightPlacementSwapPositionsAcceptMessage() { }

        public GameFightPlacementSwapPositionsAcceptMessage(int requestId) {
            this.requestId = requestId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.requestId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requestId = reader.ReadInt();

            if (this.requestId < 0)
                throw new Exception("Forbidden value on requestId = " + this.requestId + ", it doesn't respect the following condition : requestId < 0");
        }
    }
}