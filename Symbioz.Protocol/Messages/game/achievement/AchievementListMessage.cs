using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AchievementListMessage : Message {
        public const ushort Id = 6205;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] finishedAchievementsIds;
        public AchievementRewardable[] rewardableAchievements;


        public AchievementListMessage() { }

        public AchievementListMessage(ushort[] finishedAchievementsIds, AchievementRewardable[] rewardableAchievements) {
            this.finishedAchievementsIds = finishedAchievementsIds;
            this.rewardableAchievements = rewardableAchievements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.finishedAchievementsIds.Length);
            foreach (var entry in this.finishedAchievementsIds) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.rewardableAchievements.Length);
            foreach (var entry in this.rewardableAchievements) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.finishedAchievementsIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.finishedAchievementsIds[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.rewardableAchievements = new AchievementRewardable[limit];
            for (int i = 0; i < limit; i++) {
                this.rewardableAchievements[i] = new AchievementRewardable();
                this.rewardableAchievements[i].Deserialize(reader);
            }
        }
    }
}