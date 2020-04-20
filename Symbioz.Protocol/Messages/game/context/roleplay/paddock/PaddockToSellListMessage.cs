using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PaddockToSellListMessage : Message {
        public const ushort Id = 6138;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort pageIndex;
        public ushort totalPage;
        public PaddockInformationsForSell[] paddockList;


        public PaddockToSellListMessage() { }

        public PaddockToSellListMessage(ushort pageIndex, ushort totalPage, PaddockInformationsForSell[] paddockList) {
            this.pageIndex = pageIndex;
            this.totalPage = totalPage;
            this.paddockList = paddockList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.pageIndex);
            writer.WriteVarUhShort(this.totalPage);
            writer.WriteUShort((ushort) this.paddockList.Length);
            foreach (var entry in this.paddockList) {
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
            this.paddockList = new PaddockInformationsForSell[limit];
            for (int i = 0; i < limit; i++) {
                this.paddockList[i] = new PaddockInformationsForSell();
                this.paddockList[i].Deserialize(reader);
            }
        }
    }
}