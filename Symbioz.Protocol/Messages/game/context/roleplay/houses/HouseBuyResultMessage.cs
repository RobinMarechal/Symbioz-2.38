using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseBuyResultMessage : Message {
        public const ushort Id = 5735;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint houseId;
        public bool bought;
        public uint realPrice;


        public HouseBuyResultMessage() { }

        public HouseBuyResultMessage(uint houseId, bool bought, uint realPrice) {
            this.houseId = houseId;
            this.bought = bought;
            this.realPrice = realPrice;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.houseId);
            writer.WriteBoolean(this.bought);
            writer.WriteVarUhInt(this.realPrice);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.houseId = reader.ReadVarUhInt();

            if (this.houseId < 0)
                throw new Exception("Forbidden value on houseId = " + this.houseId + ", it doesn't respect the following condition : houseId < 0");
            this.bought = reader.ReadBoolean();
            this.realPrice = reader.ReadVarUhInt();

            if (this.realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + this.realPrice + ", it doesn't respect the following condition : realPrice < 0");
        }
    }
}