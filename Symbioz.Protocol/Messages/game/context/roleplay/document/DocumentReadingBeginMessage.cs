using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DocumentReadingBeginMessage : Message {
        public const ushort Id = 5675;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort documentId;


        public DocumentReadingBeginMessage() { }

        public DocumentReadingBeginMessage(ushort documentId) {
            this.documentId = documentId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.documentId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.documentId = reader.ReadVarUhShort();

            if (this.documentId < 0)
                throw new Exception("Forbidden value on documentId = " + this.documentId + ", it doesn't respect the following condition : documentId < 0");
        }
    }
}