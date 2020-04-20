using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LockableShowCodeDialogMessage : Message {
        public const ushort Id = 5740;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool changeOrUse;
        public sbyte codeSize;


        public LockableShowCodeDialogMessage() { }

        public LockableShowCodeDialogMessage(bool changeOrUse, sbyte codeSize) {
            this.changeOrUse = changeOrUse;
            this.codeSize = codeSize;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.changeOrUse);
            writer.WriteSByte(this.codeSize);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.changeOrUse = reader.ReadBoolean();
            this.codeSize = reader.ReadSByte();

            if (this.codeSize < 0)
                throw new Exception("Forbidden value on codeSize = " + this.codeSize + ", it doesn't respect the following condition : codeSize < 0");
        }
    }
}