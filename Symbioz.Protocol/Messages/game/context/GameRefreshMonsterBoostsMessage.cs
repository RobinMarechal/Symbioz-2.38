using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRefreshMonsterBoostsMessage : Message {
        public const ushort Id = 6618;

        public override ushort MessageId {
            get { return Id; }
        }

        public MonsterBoosts[] monsterBoosts;
        public MonsterBoosts[] familyBoosts;


        public GameRefreshMonsterBoostsMessage() { }

        public GameRefreshMonsterBoostsMessage(MonsterBoosts[] monsterBoosts, MonsterBoosts[] familyBoosts) {
            this.monsterBoosts = monsterBoosts;
            this.familyBoosts = familyBoosts;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.monsterBoosts.Length);
            foreach (var entry in this.monsterBoosts) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.familyBoosts.Length);
            foreach (var entry in this.familyBoosts) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.monsterBoosts = new MonsterBoosts[limit];
            for (int i = 0; i < limit; i++) {
                this.monsterBoosts[i] = new MonsterBoosts();
                this.monsterBoosts[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.familyBoosts = new MonsterBoosts[limit];
            for (int i = 0; i < limit; i++) {
                this.familyBoosts[i] = new MonsterBoosts();
                this.familyBoosts[i].Deserialize(reader);
            }
        }
    }
}