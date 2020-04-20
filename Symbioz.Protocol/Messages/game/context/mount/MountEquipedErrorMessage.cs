using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountEquipedErrorMessage : Message {
        public const ushort Id = 5963;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte errorType;


        public MountEquipedErrorMessage() { }

        public MountEquipedErrorMessage(sbyte errorType) {
            this.errorType = errorType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.errorType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.errorType = reader.ReadSByte();

            if (this.errorType < 0)
                throw new Exception("Forbidden value on errorType = " + this.errorType + ", it doesn't respect the following condition : errorType < 0");
        }
    }
}