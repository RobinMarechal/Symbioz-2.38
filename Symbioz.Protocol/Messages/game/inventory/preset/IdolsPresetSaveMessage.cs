using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolsPresetSaveMessage : Message {
        public const ushort Id = 6603;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public sbyte symbolId;


        public IdolsPresetSaveMessage() { }

        public IdolsPresetSaveMessage(sbyte presetId, sbyte symbolId) {
            this.presetId = presetId;
            this.symbolId = symbolId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteSByte(this.symbolId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.symbolId = reader.ReadSByte();

            if (this.symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + this.symbolId + ", it doesn't respect the following condition : symbolId < 0");
        }
    }
}