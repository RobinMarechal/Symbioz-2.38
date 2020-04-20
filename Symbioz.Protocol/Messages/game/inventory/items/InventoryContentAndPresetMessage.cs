using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryContentAndPresetMessage : InventoryContentMessage {
        public const ushort Id = 6162;

        public override ushort MessageId {
            get { return Id; }
        }

        public Preset[] presets;
        public IdolsPreset[] idolsPresets;


        public InventoryContentAndPresetMessage() { }

        public InventoryContentAndPresetMessage(ObjectItem[] objects, uint kamas, Preset[] presets, IdolsPreset[] idolsPresets)
            : base(objects, kamas) {
            this.presets = presets;
            this.idolsPresets = idolsPresets;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.presets.Length);
            foreach (var entry in this.presets) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.idolsPresets.Length);
            foreach (var entry in this.idolsPresets) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.presets = new Preset[limit];
            for (int i = 0; i < limit; i++) {
                this.presets[i] = new Preset();
                this.presets[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.idolsPresets = new IdolsPreset[limit];
            for (int i = 0; i < limit; i++) {
                this.idolsPresets[i] = new IdolsPreset();
                this.idolsPresets[i].Deserialize(reader);
            }
        }
    }
}