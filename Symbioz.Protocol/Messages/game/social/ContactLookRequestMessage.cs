using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ContactLookRequestMessage : Message {
        public const ushort Id = 5932;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte requestId;
        public sbyte contactType;


        public ContactLookRequestMessage() { }

        public ContactLookRequestMessage(byte requestId, sbyte contactType) {
            this.requestId = requestId;
            this.contactType = contactType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.requestId);
            writer.WriteSByte(this.contactType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requestId = reader.ReadByte();

            if (this.requestId < 0 || this.requestId > 255)
                throw new Exception("Forbidden value on requestId = " + this.requestId + ", it doesn't respect the following condition : requestId < 0 || requestId > 255");
            this.contactType = reader.ReadSByte();

            if (this.contactType < 0)
                throw new Exception("Forbidden value on contactType = " + this.contactType + ", it doesn't respect the following condition : contactType < 0");
        }
    }
}