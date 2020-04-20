using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaUpdatePlayerInfosMessage : Message {
        public const ushort Id = 6301;

        public override ushort MessageId {
            get { return Id; }
        }

        public ArenaRankInfos solo;


        public GameRolePlayArenaUpdatePlayerInfosMessage() { }

        public GameRolePlayArenaUpdatePlayerInfosMessage(ArenaRankInfos solo) {
            this.solo = solo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.solo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.solo = new ArenaRankInfos();
            this.solo.Deserialize(reader);
        }
    }
}