using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightSpellCastMessage : AbstractGameActionFightTargetedAbilityMessage {
        public const ushort Id = 1010;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort spellId;
        public sbyte spellLevel;
        public short[] portalsIds;


        public GameActionFightSpellCastMessage() { }

        public GameActionFightSpellCastMessage(ushort actionId,
                                               double sourceId,
                                               bool silentCast,
                                               bool verboseCast,
                                               double targetId,
                                               short destinationCellId,
                                               sbyte critical,
                                               ushort spellId,
                                               sbyte spellLevel,
                                               short[] portalsIds)
            : base(actionId, sourceId, silentCast, verboseCast, targetId, destinationCellId, critical) {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
            this.portalsIds = portalsIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.spellId);
            writer.WriteSByte(this.spellLevel);
            writer.WriteUShort((ushort) this.portalsIds.Length);
            foreach (var entry in this.portalsIds) {
                writer.WriteShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
            this.spellLevel = reader.ReadSByte();

            if (this.spellLevel < 1 || this.spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + this.spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
            var limit = reader.ReadUShort();
            this.portalsIds = new short[limit];
            for (int i = 0; i < limit; i++) {
                this.portalsIds[i] = reader.ReadShort();
            }
        }
    }
}