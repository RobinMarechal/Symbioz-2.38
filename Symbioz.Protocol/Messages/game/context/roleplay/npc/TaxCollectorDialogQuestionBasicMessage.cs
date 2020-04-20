using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorDialogQuestionBasicMessage : Message {
        public const ushort Id = 5619;

        public override ushort MessageId {
            get { return Id; }
        }

        public BasicGuildInformations guildInfo;


        public TaxCollectorDialogQuestionBasicMessage() { }

        public TaxCollectorDialogQuestionBasicMessage(BasicGuildInformations guildInfo) {
            this.guildInfo = guildInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.guildInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildInfo = new BasicGuildInformations();
            this.guildInfo.Deserialize(reader);
        }
    }
}