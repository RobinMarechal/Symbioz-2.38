using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildHouseRemoveMessage : Message {
        public const ushort Id = 6180;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint houseId;


        public GuildHouseRemoveMessage() { }

        public GuildHouseRemoveMessage(uint houseId) {
            this.houseId = houseId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.houseId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.houseId = reader.ReadVarUhInt();

            if (this.houseId < 0)
                throw new Exception("Forbidden value on houseId = " + this.houseId + ", it doesn't respect the following condition : houseId < 0");
        }
    }
}