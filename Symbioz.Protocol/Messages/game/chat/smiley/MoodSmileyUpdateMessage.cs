using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MoodSmileyUpdateMessage : Message {
        public const ushort Id = 6388;

        public override ushort MessageId {
            get { return Id; }
        }

        public int accountId;
        public ulong playerId;
        public ushort smileyId;


        public MoodSmileyUpdateMessage() { }

        public MoodSmileyUpdateMessage(int accountId, ulong playerId, ushort smileyId) {
            this.accountId = accountId;
            this.playerId = playerId;
            this.smileyId = smileyId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.accountId);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteVarUhShort(this.smileyId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.smileyId = reader.ReadVarUhShort();

            if (this.smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + this.smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
    }
}