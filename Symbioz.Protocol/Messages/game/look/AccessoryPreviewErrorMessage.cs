using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AccessoryPreviewErrorMessage : Message {
        public const ushort Id = 6521;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte error;


        public AccessoryPreviewErrorMessage() { }

        public AccessoryPreviewErrorMessage(sbyte error) {
            this.error = error;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.error);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.error = reader.ReadSByte();

            if (this.error < 0)
                throw new Exception("Forbidden value on error = " + this.error + ", it doesn't respect the following condition : error < 0");
        }
    }
}