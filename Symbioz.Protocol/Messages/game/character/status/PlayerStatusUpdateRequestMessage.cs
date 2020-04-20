using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PlayerStatusUpdateRequestMessage : Message {
        public const ushort Id = 6387;

        public override ushort MessageId {
            get { return Id; }
        }

        public PlayerStatus status;


        public PlayerStatusUpdateRequestMessage() { }

        public PlayerStatusUpdateRequestMessage(PlayerStatus status) {
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.status.TypeId);
            this.status.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.status = ProtocolTypeManager.GetInstance<PlayerStatus>(reader.ReadShort());
            this.status.Deserialize(reader);
        }
    }
}