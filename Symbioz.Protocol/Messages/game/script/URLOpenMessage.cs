using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class URLOpenMessage : Message {
        public const ushort Id = 6266;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte urlId;


        public URLOpenMessage() { }

        public URLOpenMessage(sbyte urlId) {
            this.urlId = urlId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.urlId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.urlId = reader.ReadSByte();

            if (this.urlId < 0)
                throw new Exception("Forbidden value on urlId = " + this.urlId + ", it doesn't respect the following condition : urlId < 0");
        }
    }
}