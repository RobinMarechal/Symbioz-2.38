// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamMemberTaxCollectorInformations : FightTeamMemberInformations {
        public const short Id = 177;

        public override short TypeId {
            get { return Id; }
        }

        public ushort firstNameId;
        public ushort lastNameId;
        public byte level;
        public uint guildId;
        public uint uid;


        public FightTeamMemberTaxCollectorInformations() { }

        public FightTeamMemberTaxCollectorInformations(double id, ushort firstNameId, ushort lastNameId, byte level, uint guildId, uint uid)
            : base(id) {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.level = level;
            this.guildId = guildId;
            this.uid = uid;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.firstNameId);
            writer.WriteVarUhShort(this.lastNameId);
            writer.WriteByte(this.level);
            writer.WriteVarUhInt(this.guildId);
            writer.WriteVarUhInt(this.uid);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.firstNameId = reader.ReadVarUhShort();

            if (this.firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + this.firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            this.lastNameId = reader.ReadVarUhShort();

            if (this.lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + this.lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            this.level = reader.ReadByte();

            if (this.level < 1 || this.level > 200)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 1 || level > 200");
            this.guildId = reader.ReadVarUhInt();

            if (this.guildId < 0)
                throw new Exception("Forbidden value on guildId = " + this.guildId + ", it doesn't respect the following condition : guildId < 0");
            this.uid = reader.ReadVarUhInt();

            if (this.uid < 0)
                throw new Exception("Forbidden value on uid = " + this.uid + ", it doesn't respect the following condition : uid < 0");
        }
    }
}