using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class WarnOnPermaDeathStateMessage : Message {
        public const ushort Id = 6513;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool enable;


        public WarnOnPermaDeathStateMessage() { }

        public WarnOnPermaDeathStateMessage(bool enable) {
            this.enable = enable;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.enable);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.enable = reader.ReadBoolean();
        }
    }
}