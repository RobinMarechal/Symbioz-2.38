using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayPlayerFightFriendlyRequestedMessage : Message {
        public const ushort Id = 5937;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public ulong sourceId;
        public ulong targetId;


        public GameRolePlayPlayerFightFriendlyRequestedMessage() { }

        public GameRolePlayPlayerFightFriendlyRequestedMessage(int fightId, ulong sourceId, ulong targetId) {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteVarUhLong(this.sourceId);
            writer.WriteVarUhLong(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.sourceId = reader.ReadVarUhLong();

            if (this.sourceId < 0 || this.sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + this.sourceId + ", it doesn't respect the following condition : sourceId < 0 || sourceId > 9007199254740990");
            this.targetId = reader.ReadVarUhLong();

            if (this.targetId < 0 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
        }
    }
}