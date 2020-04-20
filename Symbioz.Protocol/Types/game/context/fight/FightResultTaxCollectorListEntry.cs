// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightResultTaxCollectorListEntry : FightResultFighterListEntry {
        public const short Id = 84;

        public override short TypeId {
            get { return Id; }
        }

        public byte level;
        public BasicGuildInformations guildInfo;
        public int experienceForGuild;


        public FightResultTaxCollectorListEntry() { }

        public FightResultTaxCollectorListEntry(ushort outcome, sbyte wave, FightLoot rewards, double id, bool alive, byte level, BasicGuildInformations guildInfo, int experienceForGuild)
            : base(outcome, wave, rewards, id, alive) {
            this.level = level;
            this.guildInfo = guildInfo;
            this.experienceForGuild = experienceForGuild;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteByte(this.level);
            this.guildInfo.Serialize(writer);
            writer.WriteInt(this.experienceForGuild);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.level = reader.ReadByte();

            if (this.level < 1 || this.level > 200)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 1 || level > 200");
            this.guildInfo = new BasicGuildInformations();
            this.guildInfo.Deserialize(reader);
            this.experienceForGuild = reader.ReadInt();
        }
    }
}