using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayPlayerFightFriendlyAnsweredMessage : Message {
        public const ushort Id = 5733;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public ulong sourceId;
        public ulong targetId;
        public bool accept;


        public GameRolePlayPlayerFightFriendlyAnsweredMessage() { }

        public GameRolePlayPlayerFightFriendlyAnsweredMessage(int fightId, ulong sourceId, ulong targetId, bool accept) {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
            this.accept = accept;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteVarUhLong(this.sourceId);
            writer.WriteVarUhLong(this.targetId);
            writer.WriteBoolean(this.accept);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();
            this.sourceId = reader.ReadVarUhLong();

            if (this.sourceId < 0 || this.sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + this.sourceId + ", it doesn't respect the following condition : sourceId < 0 || sourceId > 9007199254740990");
            this.targetId = reader.ReadVarUhLong();

            if (this.targetId < 0 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
            this.accept = reader.ReadBoolean();
        }
    }
}