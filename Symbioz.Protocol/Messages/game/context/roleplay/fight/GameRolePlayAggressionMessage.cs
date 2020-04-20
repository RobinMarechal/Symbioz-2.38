using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayAggressionMessage : Message {
        public const ushort Id = 6073;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong attackerId;
        public ulong defenderId;


        public GameRolePlayAggressionMessage() { }

        public GameRolePlayAggressionMessage(ulong attackerId, ulong defenderId) {
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.attackerId);
            writer.WriteVarUhLong(this.defenderId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.attackerId = reader.ReadVarUhLong();

            if (this.attackerId < 0 || this.attackerId > 9007199254740990)
                throw new Exception("Forbidden value on attackerId = " + this.attackerId + ", it doesn't respect the following condition : attackerId < 0 || attackerId > 9007199254740990");
            this.defenderId = reader.ReadVarUhLong();

            if (this.defenderId < 0 || this.defenderId > 9007199254740990)
                throw new Exception("Forbidden value on defenderId = " + this.defenderId + ", it doesn't respect the following condition : defenderId < 0 || defenderId > 9007199254740990");
        }
    }
}