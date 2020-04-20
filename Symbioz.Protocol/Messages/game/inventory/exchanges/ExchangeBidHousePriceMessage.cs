using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHousePriceMessage : Message {
        public const ushort Id = 5805;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort genId;


        public ExchangeBidHousePriceMessage() { }

        public ExchangeBidHousePriceMessage(ushort genId) {
            this.genId = genId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.genId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.genId = reader.ReadVarUhShort();

            if (this.genId < 0)
                throw new Exception("Forbidden value on genId = " + this.genId + ", it doesn't respect the following condition : genId < 0");
        }
    }
}