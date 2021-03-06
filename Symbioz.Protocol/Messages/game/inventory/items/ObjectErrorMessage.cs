using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectErrorMessage : Message {
        public const ushort Id = 3004;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte reason;


        public ObjectErrorMessage() { }

        public ObjectErrorMessage(sbyte reason) {
            this.reason = reason;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.reason);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.reason = reader.ReadSByte();
        }
    }
}