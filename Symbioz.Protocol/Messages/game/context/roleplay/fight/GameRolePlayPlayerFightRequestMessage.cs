using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayPlayerFightRequestMessage : Message {
        public const ushort Id = 5731;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong targetId;
        public short targetCellId;
        public bool friendly;


        public GameRolePlayPlayerFightRequestMessage() { }

        public GameRolePlayPlayerFightRequestMessage(ulong targetId, short targetCellId, bool friendly) {
            this.targetId = targetId;
            this.targetCellId = targetCellId;
            this.friendly = friendly;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.targetId);
            writer.WriteShort(this.targetCellId);
            writer.WriteBoolean(this.friendly);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.targetId = reader.ReadVarUhLong();

            if (this.targetId < 0 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
            this.targetCellId = reader.ReadShort();

            if (this.targetCellId < -1 || this.targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + this.targetCellId + ", it doesn't respect the following condition : targetCellId < -1 || targetCellId > 559");
            this.friendly = reader.ReadBoolean();
        }
    }
}