using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FriendAddedMessage : Message {
        public const ushort Id = 5599;

        public override ushort MessageId {
            get { return Id; }
        }

        public FriendInformations friendAdded;


        public FriendAddedMessage() { }

        public FriendAddedMessage(FriendInformations friendAdded) {
            this.friendAdded = friendAdded;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.friendAdded.TypeId);
            this.friendAdded.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.friendAdded = ProtocolTypeManager.GetInstance<FriendInformations>(reader.ReadShort());
            this.friendAdded.Deserialize(reader);
        }
    }
}