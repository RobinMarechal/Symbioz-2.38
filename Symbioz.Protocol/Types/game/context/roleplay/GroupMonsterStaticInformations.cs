// Generated on 06/04/2015 18:45:27

using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GroupMonsterStaticInformations {
        public const short Id = 140;

        public virtual short TypeId {
            get { return Id; }
        }

        public MonsterInGroupLightInformations mainCreatureLightInfos;
        public IEnumerable<MonsterInGroupInformations> underlings;


        public GroupMonsterStaticInformations() { }

        public GroupMonsterStaticInformations(MonsterInGroupLightInformations mainCreatureLightInfos, IEnumerable<MonsterInGroupInformations> underlings) {
            this.mainCreatureLightInfos = mainCreatureLightInfos;
            this.underlings = underlings;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            this.mainCreatureLightInfos.Serialize(writer);
            writer.WriteUShort((ushort) this.underlings.Count());
            foreach (var entry in this.underlings) {
                entry.Serialize(writer);
            }
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.mainCreatureLightInfos = new MonsterInGroupLightInformations();
            this.mainCreatureLightInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.underlings = new MonsterInGroupInformations[limit];
            for (int i = 0; i < limit; i++) {
                (this.underlings as MonsterInGroupInformations[])[i] = new MonsterInGroupInformations();
                (this.underlings as MonsterInGroupInformations[])[i].Deserialize(reader);
            }
        }
    }
}