using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlaySpellAnimMessage : Message {
        public const ushort Id = 6114;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong casterId;
        public ushort targetCellId;
        public ushort spellId;
        public sbyte spellLevel;


        public GameRolePlaySpellAnimMessage() { }

        public GameRolePlaySpellAnimMessage(ulong casterId, ushort targetCellId, ushort spellId, sbyte spellLevel) {
            this.casterId = casterId;
            this.targetCellId = targetCellId;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.casterId);
            writer.WriteVarUhShort(this.targetCellId);
            writer.WriteVarUhShort(this.spellId);
            writer.WriteSByte(this.spellLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.casterId = reader.ReadVarUhLong();

            if (this.casterId < 0 || this.casterId > 9007199254740990)
                throw new Exception("Forbidden value on casterId = " + this.casterId + ", it doesn't respect the following condition : casterId < 0 || casterId > 9007199254740990");
            this.targetCellId = reader.ReadVarUhShort();

            if (this.targetCellId < 0 || this.targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + this.targetCellId + ", it doesn't respect the following condition : targetCellId < 0 || targetCellId > 559");
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.spellLevel = reader.ReadSByte();

            if (this.spellLevel < 1 || this.spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + this.spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
    }
}