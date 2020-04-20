using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildPaddockBoughtMessage : Message {
        public const ushort Id = 5952;

        public override ushort MessageId {
            get { return Id; }
        }

        public PaddockContentInformations paddockInfo;


        public GuildPaddockBoughtMessage() { }

        public GuildPaddockBoughtMessage(PaddockContentInformations paddockInfo) {
            this.paddockInfo = paddockInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.paddockInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.paddockInfo = new PaddockContentInformations();
            this.paddockInfo.Deserialize(reader);
        }
    }
}