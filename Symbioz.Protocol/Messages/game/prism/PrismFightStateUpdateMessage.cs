using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismFightStateUpdateMessage : Message {
        public const ushort Id = 6040;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte state;


        public PrismFightStateUpdateMessage() { }

        public PrismFightStateUpdateMessage(sbyte state) {
            this.state = state;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.state);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.state = reader.ReadSByte();

            if (this.state < 0)
                throw new Exception("Forbidden value on state = " + this.state + ", it doesn't respect the following condition : state < 0");
        }
    }
}