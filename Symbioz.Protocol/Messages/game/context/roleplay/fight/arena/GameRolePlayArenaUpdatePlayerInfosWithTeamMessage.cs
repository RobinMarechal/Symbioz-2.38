using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaUpdatePlayerInfosWithTeamMessage : GameRolePlayArenaUpdatePlayerInfosMessage {
        public const ushort Id = 6640;

        public override ushort MessageId {
            get { return Id; }
        }

        public ArenaRankInfos team;


        public GameRolePlayArenaUpdatePlayerInfosWithTeamMessage() { }

        public GameRolePlayArenaUpdatePlayerInfosWithTeamMessage(ArenaRankInfos solo, ArenaRankInfos team)
            : base(solo) {
            this.team = team;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.team.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.team = new ArenaRankInfos();
            this.team.Deserialize(reader);
        }
    }
}