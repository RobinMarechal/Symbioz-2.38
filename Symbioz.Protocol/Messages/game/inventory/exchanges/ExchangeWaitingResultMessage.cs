using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeWaitingResultMessage : Message {
        public const ushort Id = 5786;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool bwait;


        public ExchangeWaitingResultMessage() { }

        public ExchangeWaitingResultMessage(bool bwait) {
            this.bwait = bwait;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.bwait);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.bwait = reader.ReadBoolean();
        }
    }
}