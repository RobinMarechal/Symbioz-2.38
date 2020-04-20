using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseToSellListMessage : Message {
        public const ushort Id = 6140;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort pageIndex;
        public ushort totalPage;
        public HouseInformationsForSell[] houseList;


        public HouseToSellListMessage() { }

        public HouseToSellListMessage(ushort pageIndex, ushort totalPage, HouseInformationsForSell[] houseList) {
            this.pageIndex = pageIndex;
            this.totalPage = totalPage;
            this.houseList = houseList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.pageIndex);
            writer.WriteVarUhShort(this.totalPage);
            writer.WriteUShort((ushort) this.houseList.Length);
            foreach (var entry in this.houseList) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.pageIndex = reader.ReadVarUhShort();

            if (this.pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + this.pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
            this.totalPage = reader.ReadVarUhShort();

            if (this.totalPage < 0)
                throw new Exception("Forbidden value on totalPage = " + this.totalPage + ", it doesn't respect the following condition : totalPage < 0");
            var limit = reader.ReadUShort();
            this.houseList = new HouseInformationsForSell[limit];
            for (int i = 0; i < limit; i++) {
                this.houseList[i] = new HouseInformationsForSell();
                this.houseList[i].Deserialize(reader);
            }
        }
    }
}