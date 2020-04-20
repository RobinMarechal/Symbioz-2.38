using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFightPlayersHelpersJoinMessage : Message {
        public const ushort Id = 5720;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public CharacterMinimalPlusLookInformations playerInfo;


        public GuildFightPlayersHelpersJoinMessage() { }

        public GuildFightPlayersHelpersJoinMessage(int fightId, CharacterMinimalPlusLookInformations playerInfo) {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            this.playerInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.playerInfo = new CharacterMinimalPlusLookInformations();
            this.playerInfo.Deserialize(reader);
        }
    }
}