using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DownloadErrorMessage : Message {
        public const ushort Id = 1513;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte errorId;
        public string message;
        public string helpUrl;


        public DownloadErrorMessage() { }

        public DownloadErrorMessage(sbyte errorId, string message, string helpUrl) {
            this.errorId = errorId;
            this.message = message;
            this.helpUrl = helpUrl;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.errorId);
            writer.WriteUTF(this.message);
            writer.WriteUTF(this.helpUrl);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.errorId = reader.ReadSByte();

            if (this.errorId < 0)
                throw new Exception("Forbidden value on errorId = " + this.errorId + ", it doesn't respect the following condition : errorId < 0");
            this.message = reader.ReadUTF();
            this.helpUrl = reader.ReadUTF();
        }
    }
}