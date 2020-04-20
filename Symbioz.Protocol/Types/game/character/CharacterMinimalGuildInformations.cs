// Generated on 04/27/2016 01:13:09

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class CharacterMinimalGuildInformations : CharacterMinimalPlusLookInformations {
        public const short Id = 445;

        public override short TypeId {
            get { return Id; }
        }

        public BasicGuildInformations guild;


        public CharacterMinimalGuildInformations() { }

        public CharacterMinimalGuildInformations(ulong id, string name, byte level, EntityLook entityLook, BasicGuildInformations guild)
            : base(id, name, level, entityLook) {
            this.guild = guild;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.guild.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.guild = new BasicGuildInformations();
            this.guild.Deserialize(reader);
        }
    }
}