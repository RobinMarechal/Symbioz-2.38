using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayPlayerFightFriendlyAnswerMessage : Message {
        public const ushort Id = 5732;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public bool accept;


        public GameRolePlayPlayerFightFriendlyAnswerMessage() { }

        public GameRolePlayPlayerFightFriendlyAnswerMessage(int fightId, bool accept) {
            this.fightId = fightId;
            this.accept = accept;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteBoolean(this.accept);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();
            this.accept = reader.ReadBoolean();
        }
    }
}