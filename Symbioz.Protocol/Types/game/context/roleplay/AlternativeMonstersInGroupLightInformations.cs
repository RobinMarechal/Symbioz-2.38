// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class AlternativeMonstersInGroupLightInformations {
        public const short Id = 394;

        public virtual short TypeId {
            get { return Id; }
        }

        public int playerCount;
        public MonsterInGroupLightInformations[] monsters;


        public AlternativeMonstersInGroupLightInformations() { }

        public AlternativeMonstersInGroupLightInformations(int playerCount, MonsterInGroupLightInformations[] monsters) {
            this.playerCount = playerCount;
            this.monsters = monsters;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.playerCount);
            writer.WriteUShort((ushort) this.monsters.Length);
            foreach (var entry in this.monsters) {
                entry.Serialize(writer);
            }
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.playerCount = reader.ReadInt();
            var limit = reader.ReadUShort();
            this.monsters = new MonsterInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.monsters[i] = new MonsterInGroupLightInformations();
                this.monsters[i].Deserialize(reader);
            }
        }
    }
}