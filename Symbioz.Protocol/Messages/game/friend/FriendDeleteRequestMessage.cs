using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FriendDeleteRequestMessage : Message {
        public const ushort Id = 5603;

        public override ushort MessageId {
            get { return Id; }
        }

        public int accountId;


        public FriendDeleteRequestMessage() { }

        public FriendDeleteRequestMessage(int accountId) {
            this.accountId = accountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.accountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
        }
    }
}