using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkRecycleTradeMessage : Message {
        public const ushort Id = 6600;

        public override ushort MessageId {
            get { return Id; }
        }

        public short percentToPrism;
        public short percentToPlayer;


        public ExchangeStartOkRecycleTradeMessage() { }

        public ExchangeStartOkRecycleTradeMessage(short percentToPrism, short percentToPlayer) {
            this.percentToPrism = percentToPrism;
            this.percentToPlayer = percentToPlayer;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.percentToPrism);
            writer.WriteShort(this.percentToPlayer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.percentToPrism = reader.ReadShort();

            if (this.percentToPrism < 0)
                throw new Exception("Forbidden value on percentToPrism = " + this.percentToPrism + ", it doesn't respect the following condition : percentToPrism < 0");
            this.percentToPlayer = reader.ReadShort();

            if (this.percentToPlayer < 0)
                throw new Exception("Forbidden value on percentToPlayer = " + this.percentToPlayer + ", it doesn't respect the following condition : percentToPlayer < 0");
        }
    }
}