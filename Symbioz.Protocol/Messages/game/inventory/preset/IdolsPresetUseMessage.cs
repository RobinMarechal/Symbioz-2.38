using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolsPresetUseMessage : Message {
        public const ushort Id = 6615;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public bool party;


        public IdolsPresetUseMessage() { }

        public IdolsPresetUseMessage(sbyte presetId, bool party) {
            this.presetId = presetId;
            this.party = party;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteBoolean(this.party);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.party = reader.ReadBoolean();
        }
    }
}