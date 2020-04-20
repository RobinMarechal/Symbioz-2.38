using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartedMessage : Message {
        public const ushort Id = 5512;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte exchangeType;


        public ExchangeStartedMessage() { }

        public ExchangeStartedMessage(sbyte exchangeType) {
            this.exchangeType = exchangeType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.exchangeType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.exchangeType = reader.ReadSByte();
        }
    }
}