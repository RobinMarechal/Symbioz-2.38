using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseSoldMessage : Message {
        public const ushort Id = 5737;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint houseId;
        public uint realPrice;
        public string buyerName;


        public HouseSoldMessage() { }

        public HouseSoldMessage(uint houseId, uint realPrice, string buyerName) {
            this.houseId = houseId;
            this.realPrice = realPrice;
            this.buyerName = buyerName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.houseId);
            writer.WriteVarUhInt(this.realPrice);
            writer.WriteUTF(this.buyerName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.houseId = reader.ReadVarUhInt();

            if (this.houseId < 0)
                throw new Exception("Forbidden value on houseId = " + this.houseId + ", it doesn't respect the following condition : houseId < 0");
            this.realPrice = reader.ReadVarUhInt();

            if (this.realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + this.realPrice + ", it doesn't respect the following condition : realPrice < 0");
            this.buyerName = reader.ReadUTF();
        }
    }
}