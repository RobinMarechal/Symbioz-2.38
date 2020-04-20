using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeCraftResultMessage : Message {
        public const ushort Id = 5790;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte craftResult;


        public ExchangeCraftResultMessage() { }

        public ExchangeCraftResultMessage(sbyte craftResult) {
            this.craftResult = craftResult;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.craftResult);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.craftResult = reader.ReadSByte();

            if (this.craftResult < 0)
                throw new Exception("Forbidden value on craftResult = " + this.craftResult + ", it doesn't respect the following condition : craftResult < 0");
        }
    }
}