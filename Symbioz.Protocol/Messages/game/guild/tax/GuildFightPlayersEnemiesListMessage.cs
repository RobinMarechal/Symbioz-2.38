using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFightPlayersEnemiesListMessage : Message {
        public const ushort Id = 5928;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public CharacterMinimalPlusLookInformations[] playerInfo;


        public GuildFightPlayersEnemiesListMessage() { }

        public GuildFightPlayersEnemiesListMessage(int fightId, CharacterMinimalPlusLookInformations[] playerInfo) {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteUShort((ushort) this.playerInfo.Length);
            foreach (var entry in this.playerInfo) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            this.playerInfo = new CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.playerInfo[i] = new CharacterMinimalPlusLookInformations();
                this.playerInfo[i].Deserialize(reader);
            }
        }
    }
}