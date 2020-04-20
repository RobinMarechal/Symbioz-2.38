using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolsPresetUseResultMessage : Message {
        public const ushort Id = 6614;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public sbyte code;
        public ushort[] missingIdols;


        public IdolsPresetUseResultMessage() { }

        public IdolsPresetUseResultMessage(sbyte presetId, sbyte code, ushort[] missingIdols) {
            this.presetId = presetId;
            this.code = code;
            this.missingIdols = missingIdols;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteSByte(this.code);
            writer.WriteUShort((ushort) this.missingIdols.Length);
            foreach (var entry in this.missingIdols) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.code = reader.ReadSByte();

            if (this.code < 0)
                throw new Exception("Forbidden value on code = " + this.code + ", it doesn't respect the following condition : code < 0");
            var limit = reader.ReadUShort();
            this.missingIdols = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.missingIdols[i] = reader.ReadVarUhShort();
            }
        }
    }
}