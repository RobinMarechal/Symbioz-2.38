using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseListMessage : Message {
        public const ushort Id = 5807;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort id;


        public ExchangeBidHouseListMessage() { }

        public ExchangeBidHouseListMessage(ushort id) {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.id);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadVarUhShort();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
        }
    }
}