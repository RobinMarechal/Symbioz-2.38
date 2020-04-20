using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IgnoredDeleteRequestMessage : Message {
        public const ushort Id = 5680;

        public override ushort MessageId {
            get { return Id; }
        }

        public int accountId;
        public bool session;


        public IgnoredDeleteRequestMessage() { }

        public IgnoredDeleteRequestMessage(int accountId, bool session) {
            this.accountId = accountId;
            this.session = session;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.accountId);
            writer.WriteBoolean(this.session);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.session = reader.ReadBoolean();
        }
    }
}