using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolsPresetDeleteResultMessage : Message {
        public const ushort Id = 6605;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public sbyte code;


        public IdolsPresetDeleteResultMessage() { }

        public IdolsPresetDeleteResultMessage(sbyte presetId, sbyte code) {
            this.presetId = presetId;
            this.code = code;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteSByte(this.code);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.code = reader.ReadSByte();

            if (this.code < 0)
                throw new Exception("Forbidden value on code = " + this.code + ", it doesn't respect the following condition : code < 0");
        }
    }
}