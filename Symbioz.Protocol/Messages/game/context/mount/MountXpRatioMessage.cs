using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountXpRatioMessage : Message {
        public const ushort Id = 5970;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte ratio;


        public MountXpRatioMessage() { }

        public MountXpRatioMessage(sbyte ratio) {
            this.ratio = ratio;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.ratio);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.ratio = reader.ReadSByte();

            if (this.ratio < 0)
                throw new Exception("Forbidden value on ratio = " + this.ratio + ", it doesn't respect the following condition : ratio < 0");
        }
    }
}