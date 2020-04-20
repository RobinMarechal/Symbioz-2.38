using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseTypeMessage : Message {
        public const ushort Id = 5803;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint type;


        public ExchangeBidHouseTypeMessage() { }

        public ExchangeBidHouseTypeMessage(uint type) {
            this.type = type;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.type);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.type = reader.ReadVarUhInt();

            if (this.type < 0)
                throw new Exception("Forbidden value on type = " + this.type + ", it doesn't respect the following condition : type < 0");
        }
    }
}