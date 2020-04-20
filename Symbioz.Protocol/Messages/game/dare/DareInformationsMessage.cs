using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareInformationsMessage : Message {
        public const ushort Id = 6656;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareInformations dareFixedInfos;
        public DareVersatileInformations dareVersatilesInfos;


        public DareInformationsMessage() { }

        public DareInformationsMessage(DareInformations dareFixedInfos, DareVersatileInformations dareVersatilesInfos) {
            this.dareFixedInfos = dareFixedInfos;
            this.dareVersatilesInfos = dareVersatilesInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.dareFixedInfos.Serialize(writer);
            this.dareVersatilesInfos.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dareFixedInfos = new DareInformations();
            this.dareFixedInfos.Deserialize(reader);
            this.dareVersatilesInfos = new DareVersatileInformations();
            this.dareVersatilesInfos.Deserialize(reader);
        }
    }
}