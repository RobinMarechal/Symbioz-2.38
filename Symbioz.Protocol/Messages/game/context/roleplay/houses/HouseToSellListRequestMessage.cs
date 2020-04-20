using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseToSellListRequestMessage : Message {
        public const ushort Id = 6139;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort pageIndex;


        public HouseToSellListRequestMessage() { }

        public HouseToSellListRequestMessage(ushort pageIndex) {
            this.pageIndex = pageIndex;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.pageIndex);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.pageIndex = reader.ReadVarUhShort();

            if (this.pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + this.pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
        }
    }
}