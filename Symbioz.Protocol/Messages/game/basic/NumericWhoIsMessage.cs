using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NumericWhoIsMessage : Message {
        public const ushort Id = 6297;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong playerId;
        public int accountId;


        public NumericWhoIsMessage() { }

        public NumericWhoIsMessage(ulong playerId, int accountId) {
            this.playerId = playerId;
            this.accountId = accountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.playerId);
            writer.WriteInt(this.accountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
        }
    }
}