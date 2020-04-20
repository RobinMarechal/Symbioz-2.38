using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FriendDeleteResultMessage : Message {
        public const ushort Id = 5601;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool success;
        public string name;


        public FriendDeleteResultMessage() { }

        public FriendDeleteResultMessage(bool success, string name) {
            this.success = success;
            this.name = name;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.success);
            writer.WriteUTF(this.name);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.success = reader.ReadBoolean();
            this.name = reader.ReadUTF();
        }
    }
}