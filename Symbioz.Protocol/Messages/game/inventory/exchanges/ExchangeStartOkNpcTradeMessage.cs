using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkNpcTradeMessage : Message {
        public const ushort Id = 5785;

        public override ushort MessageId {
            get { return Id; }
        }

        public double npcId;


        public ExchangeStartOkNpcTradeMessage() { }

        public ExchangeStartOkNpcTradeMessage(double npcId) {
            this.npcId = npcId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.npcId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.npcId = reader.ReadDouble();

            if (this.npcId < -9007199254740990 || this.npcId > 9007199254740990)
                throw new Exception("Forbidden value on npcId = " + this.npcId + ", it doesn't respect the following condition : npcId < -9007199254740990 || npcId > 9007199254740990");
        }
    }
}