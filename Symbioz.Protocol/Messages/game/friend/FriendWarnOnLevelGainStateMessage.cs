using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FriendWarnOnLevelGainStateMessage : Message {
        public const ushort Id = 6078;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool enable;


        public FriendWarnOnLevelGainStateMessage() { }

        public FriendWarnOnLevelGainStateMessage(bool enable) {
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