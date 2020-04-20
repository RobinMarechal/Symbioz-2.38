using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFightLeaveRequestMessage : Message {
        public const ushort Id = 5715;

        public override ushort MessageId {
            get { return Id; }
        }

        public int taxCollectorId;
        public ulong characterId;


        public GuildFightLeaveRequestMessage() { }

        public GuildFightLeaveRequestMessage(int taxCollectorId, ulong characterId) {
            this.taxCollectorId = taxCollectorId;
            this.characterId = characterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.taxCollectorId);
            writer.WriteVarUhLong(this.characterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.taxCollectorId = reader.ReadInt();

            if (this.taxCollectorId < 0)
                throw new Exception("Forbidden value on taxCollectorId = " + this.taxCollectorId + ", it doesn't respect the following condition : taxCollectorId < 0");
            this.characterId = reader.ReadVarUhLong();

            if (this.characterId < 0 || this.characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + this.characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
        }
    }
}