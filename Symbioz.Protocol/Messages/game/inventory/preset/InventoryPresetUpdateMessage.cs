using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryPresetUpdateMessage : Message {
        public const ushort Id = 6171;

        public override ushort MessageId {
            get { return Id; }
        }

        public Preset preset;


        public InventoryPresetUpdateMessage() { }

        public InventoryPresetUpdateMessage(Preset preset) {
            this.preset = preset;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.preset.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.preset = new Preset();
            this.preset.Deserialize(reader);
        }
    }
}