using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PlayerStatusUpdateMessage : Message {
        public const ushort Id = 6386;

        public override ushort MessageId {
            get { return Id; }
        }

        public int accountId;
        public ulong playerId;
        public PlayerStatus status;


        public PlayerStatusUpdateMessage() { }

        public PlayerStatusUpdateMessage(int accountId, ulong playerId, PlayerStatus status) {
            this.accountId = accountId;
            this.playerId = playerId;
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.accountId);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteShort(this.status.TypeId);
            this.status.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.status = ProtocolTypeManager.GetInstance<PlayerStatus>(reader.ReadShort());
            this.status.Deserialize(reader);
        }
    }
}