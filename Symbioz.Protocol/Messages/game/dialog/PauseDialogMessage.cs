using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PauseDialogMessage : Message {
        public const ushort Id = 6012;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte dialogType;


        public PauseDialogMessage() { }

        public PauseDialogMessage(sbyte dialogType) {
            this.dialogType = dialogType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.dialogType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dialogType = reader.ReadSByte();

            if (this.dialogType < 0)
                throw new Exception("Forbidden value on dialogType = " + this.dialogType + ", it doesn't respect the following condition : dialogType < 0");
        }
    }
}