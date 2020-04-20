using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightSpellImmunityMessage : AbstractGameActionMessage {
        public const ushort Id = 6221;

        public override ushort MessageId {
            get { return Id; }
        }

        public double targetId;
        public ushort spellId;


        public GameActionFightSpellImmunityMessage() { }

        public GameActionFightSpellImmunityMessage(ushort actionId, double sourceId, double targetId, ushort spellId)
            : base(actionId, sourceId) {
            this.targetId = targetId;
            this.spellId = spellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhShort(this.spellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
        }
    }
}