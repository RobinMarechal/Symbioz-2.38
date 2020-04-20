using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseBuyResultMessage : Message {
        public const ushort Id = 6272;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint uid;
        public bool bought;


        public ExchangeBidHouseBuyResultMessage() { }

        public ExchangeBidHouseBuyResultMessage(uint uid, bool bought) {
            this.uid = uid;
            this.bought = bought;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.uid);
            writer.WriteBoolean(this.bought);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.uid = reader.ReadVarUhInt();

            if (this.uid < 0)
                throw new Exception("Forbidden value on uid = " + this.uid + ", it doesn't respect the following condition : uid < 0");
            this.bought = reader.ReadBoolean();
        }
    }
}