// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightResultPlayerListEntry : FightResultFighterListEntry {
        public const short Id = 24;

        public override short TypeId {
            get { return Id; }
        }

        public byte level;
        public FightResultAdditionalData[] additional;


        public FightResultPlayerListEntry() { }

        public FightResultPlayerListEntry(ushort outcome, sbyte wave, FightLoot rewards, double id, bool alive, byte level, FightResultAdditionalData[] additional)
            : base(outcome, wave, rewards, id, alive) {
            this.level = level;
            this.additional = additional;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteByte(this.level);
            writer.WriteUShort((ushort) this.additional.Length);
            foreach (var entry in this.additional) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.level = reader.ReadByte();

            if (this.level < 1 || this.level > 200)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 1 || level > 200");
            var limit = reader.ReadUShort();
            this.additional = new FightResultAdditionalData[limit];
            for (int i = 0; i < limit; i++) {
                this.additional[i] = ProtocolTypeManager.GetInstance<FightResultAdditionalData>(reader.ReadShort());
                this.additional[i].Deserialize(reader);
            }
        }
    }
}