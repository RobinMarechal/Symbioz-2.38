using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeIsReadyMessage : Message {
        public const ushort Id = 5509;

        public override ushort MessageId {
            get { return Id; }
        }

        public double id;
        public bool ready;


        public ExchangeIsReadyMessage() { }

        public ExchangeIsReadyMessage(double id, bool ready) {
            this.id = id;
            this.ready = ready;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
            writer.WriteBoolean(this.ready);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            this.ready = reader.ReadBoolean();
        }
    }
}