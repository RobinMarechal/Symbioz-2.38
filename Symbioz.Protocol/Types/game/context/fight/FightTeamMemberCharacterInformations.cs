// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamMemberCharacterInformations : FightTeamMemberInformations {
        public const short Id = 13;

        public override short TypeId {
            get { return Id; }
        }

        public string name;
        public byte level;


        public FightTeamMemberCharacterInformations() { }

        public FightTeamMemberCharacterInformations(double id, string name, byte level)
            : base(id) {
            this.name = name;
            this.level = level;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUTF(this.name);
            writer.WriteByte(this.level);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.name = reader.ReadUTF();
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 255)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 255");
        }
    }
}