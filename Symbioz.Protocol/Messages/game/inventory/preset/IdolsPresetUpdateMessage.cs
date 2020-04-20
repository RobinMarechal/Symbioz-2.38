using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolsPresetUpdateMessage : Message {
        public const ushort Id = 6606;

        public override ushort MessageId {
            get { return Id; }
        }

        public IdolsPreset idolsPreset;


        public IdolsPresetUpdateMessage() { }

        public IdolsPresetUpdateMessage(IdolsPreset idolsPreset) {
            this.idolsPreset = idolsPreset;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.idolsPreset.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.idolsPreset = new IdolsPreset();
            this.idolsPreset.Deserialize(reader);
        }
    }
}