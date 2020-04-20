using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeReadyMessage : Message {
        public const ushort Id = 5511;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool ready;
        public ushort step;


        public ExchangeReadyMessage() { }

        public ExchangeReadyMessage(bool ready, ushort step) {
            this.ready = ready;
            this.step = step;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.ready);
            writer.WriteVarUhShort(this.step);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.ready = reader.ReadBoolean();
            this.step = reader.ReadVarUhShort();

            if (this.step < 0)
                throw new Exception("Forbidden value on step = " + this.step + ", it doesn't respect the following condition : step < 0");
        }
    }
}