// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GroupMonsterStaticInformationsWithAlternatives : GroupMonsterStaticInformations {
        public const short Id = 396;

        public override short TypeId {
            get { return Id; }
        }

        public AlternativeMonstersInGroupLightInformations[] alternatives;


        public GroupMonsterStaticInformationsWithAlternatives() { }

        public GroupMonsterStaticInformationsWithAlternatives(MonsterInGroupLightInformations mainCreatureLightInfos, MonsterInGroupInformations[] underlings, AlternativeMonstersInGroupLightInformations[] alternatives)
            : base(mainCreatureLightInfos, underlings) {
            this.alternatives = alternatives;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.alternatives.Length);
            foreach (var entry in this.alternatives) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.alternatives = new AlternativeMonstersInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.alternatives[i] = new AlternativeMonstersInGroupLightInformations();
                this.alternatives[i].Deserialize(reader);
            }
        }
    }
}