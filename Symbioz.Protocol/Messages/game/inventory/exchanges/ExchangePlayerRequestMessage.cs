using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangePlayerRequestMessage : ExchangeRequestMessage {
        public const ushort Id = 5773;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong target;


        public ExchangePlayerRequestMessage() { }

        public ExchangePlayerRequestMessage(sbyte exchangeType, ulong target)
            : base(exchangeType) {
            this.target = target;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.target);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.target = reader.ReadVarUhLong();

            if (this.target < 0 || this.target > 9007199254740990)
                throw new Exception("Forbidden value on target = " + this.target + ", it doesn't respect the following condition : target < 0 || target > 9007199254740990");
        }
    }
}