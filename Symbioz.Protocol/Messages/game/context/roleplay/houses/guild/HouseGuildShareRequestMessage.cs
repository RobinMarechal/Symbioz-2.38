using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseGuildShareRequestMessage : Message {
        public const ushort Id = 5704;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool enable;
        public uint rights;


        public HouseGuildShareRequestMessage() { }

        public HouseGuildShareRequestMessage(bool enable, uint rights) {
            this.enable = enable;
            this.rights = rights;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.enable);
            writer.WriteVarUhInt(this.rights);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.enable = reader.ReadBoolean();
            this.rights = reader.ReadVarUhInt();

            if (this.rights < 0)
                throw new Exception("Forbidden value on rights = " + this.rights + ", it doesn't respect the following condition : rights < 0");
        }
    }
}