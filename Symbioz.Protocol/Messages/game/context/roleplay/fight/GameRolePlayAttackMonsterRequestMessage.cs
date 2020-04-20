using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayAttackMonsterRequestMessage : Message {
        public const ushort Id = 6191;

        public override ushort MessageId {
            get { return Id; }
        }

        public double monsterGroupId;


        public GameRolePlayAttackMonsterRequestMessage() { }

        public GameRolePlayAttackMonsterRequestMessage(double monsterGroupId) {
            this.monsterGroupId = monsterGroupId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.monsterGroupId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.monsterGroupId = reader.ReadDouble();

            if (this.monsterGroupId < -9007199254740990 || this.monsterGroupId > 9007199254740990)
                throw new Exception("Forbidden value on monsterGroupId = " + this.monsterGroupId + ", it doesn't respect the following condition : monsterGroupId < -9007199254740990 || monsterGroupId > 9007199254740990");
        }
    }
}