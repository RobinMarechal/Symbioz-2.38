using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryPresetItemUpdateErrorMessage : Message {
        public const ushort Id = 6211;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte code;


        public InventoryPresetItemUpdateErrorMessage() { }

        public InventoryPresetItemUpdateErrorMessage(sbyte code) {
            this.code = code;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.code);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.code = reader.ReadSByte();

            if (this.code < 0)
                throw new Exception("Forbidden value on code = " + this.code + ", it doesn't respect the following condition : code < 0");
        }
    }
}