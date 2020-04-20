using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeRequestedTradeMessage : ExchangeRequestedMessage {
        public const ushort Id = 5523;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong source;
        public ulong target;


        public ExchangeRequestedTradeMessage() { }

        public ExchangeRequestedTradeMessage(sbyte exchangeType, ulong source, ulong target)
            : base(exchangeType) {
            this.source = source;
            this.target = target;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.source);
            writer.WriteVarUhLong(this.target);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.source = reader.ReadVarUhLong();

            if (this.source < 0 || this.source > 9007199254740990)
                throw new Exception("Forbidden value on source = " + this.source + ", it doesn't respect the following condition : source < 0 || source > 9007199254740990");
            this.target = reader.ReadVarUhLong();

            if (this.target < 0 || this.target > 9007199254740990)
                throw new Exception("Forbidden value on target = " + this.target + ", it doesn't respect the following condition : target < 0 || target > 9007199254740990");
        }
    }
}