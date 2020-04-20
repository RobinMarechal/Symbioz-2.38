using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DownloadCurrentSpeedMessage : Message {
        public const ushort Id = 1511;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte downloadSpeed;


        public DownloadCurrentSpeedMessage() { }

        public DownloadCurrentSpeedMessage(sbyte downloadSpeed) {
            this.downloadSpeed = downloadSpeed;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.downloadSpeed);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.downloadSpeed = reader.ReadSByte();

            if (this.downloadSpeed < 1 || this.downloadSpeed > 10)
                throw new Exception("Forbidden value on downloadSpeed = " + this.downloadSpeed + ", it doesn't respect the following condition : downloadSpeed < 1 || downloadSpeed > 10");
        }
    }
}