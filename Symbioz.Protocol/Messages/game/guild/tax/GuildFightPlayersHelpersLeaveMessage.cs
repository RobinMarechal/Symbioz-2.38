using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFightPlayersHelpersLeaveMessage : Message {
        public const ushort Id = 5719;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public ulong playerId;


        public GuildFightPlayersHelpersLeaveMessage() { }

        public GuildFightPlayersHelpersLeaveMessage(int fightId, ulong playerId) {
            this.fightId = fightId;
            this.playerId = playerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteVarUhLong(this.playerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
        }
    }
}