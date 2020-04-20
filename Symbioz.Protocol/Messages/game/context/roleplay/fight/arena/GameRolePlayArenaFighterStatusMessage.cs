using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaFighterStatusMessage : Message {
        public const ushort Id = 6281;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public int playerId;
        public bool accepted;


        public GameRolePlayArenaFighterStatusMessage() { }

        public GameRolePlayArenaFighterStatusMessage(int fightId, int playerId, bool accepted) {
            this.fightId = fightId;
            this.playerId = playerId;
            this.accepted = accepted;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteInt(this.playerId);
            writer.WriteBoolean(this.accepted);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();
            this.playerId = reader.ReadInt();
            this.accepted = reader.ReadBoolean();
        }
    }
}