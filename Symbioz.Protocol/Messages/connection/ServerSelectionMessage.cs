using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ServerSelectionMessage : Message {
        public const ushort Id = 40;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort serverId;


        public ServerSelectionMessage() { }

        public ServerSelectionMessage(ushort serverId) {
            this.serverId = serverId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.serverId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.serverId = reader.ReadVarUhShort();

            if (this.serverId < 0)
                throw new Exception("Forbidden value on serverId = " + this.serverId + ", it doesn't respect the following condition : serverId < 0");
        }
    }
}