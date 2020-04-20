using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismUseRequestMessage : Message {
        public const ushort Id = 6041;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte moduleToUse;


        public PrismUseRequestMessage() { }

        public PrismUseRequestMessage(sbyte moduleToUse) {
            this.moduleToUse = moduleToUse;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.moduleToUse);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.moduleToUse = reader.ReadSByte();

            if (this.moduleToUse < 0)
                throw new Exception("Forbidden value on moduleToUse = " + this.moduleToUse + ", it doesn't respect the following condition : moduleToUse < 0");
        }
    }
}