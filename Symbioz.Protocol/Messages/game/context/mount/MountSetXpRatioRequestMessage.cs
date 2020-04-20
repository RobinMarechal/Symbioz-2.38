using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountSetXpRatioRequestMessage : Message {
        public const ushort Id = 5989;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte xpRatio;


        public MountSetXpRatioRequestMessage() { }

        public MountSetXpRatioRequestMessage(sbyte xpRatio) {
            this.xpRatio = xpRatio;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.xpRatio);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.xpRatio = reader.ReadSByte();

            if (this.xpRatio < 0)
                throw new Exception("Forbidden value on xpRatio = " + this.xpRatio + ", it doesn't respect the following condition : xpRatio < 0");
        }
    }
}