using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class OrnamentGainedMessage : Message {
        public const ushort Id = 6368;

        public override ushort MessageId {
            get { return Id; }
        }

        public short ornamentId;


        public OrnamentGainedMessage() { }

        public OrnamentGainedMessage(short ornamentId) {
            this.ornamentId = ornamentId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.ornamentId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.ornamentId = reader.ReadShort();

            if (this.ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + this.ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
        }
    }
}