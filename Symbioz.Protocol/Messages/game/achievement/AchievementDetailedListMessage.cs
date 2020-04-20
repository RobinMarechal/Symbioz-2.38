using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AchievementDetailedListMessage : Message {
        public const ushort Id = 6358;

        public override ushort MessageId {
            get { return Id; }
        }

        public Achievement[] startedAchievements;
        public Achievement[] finishedAchievements;


        public AchievementDetailedListMessage() { }

        public AchievementDetailedListMessage(Achievement[] startedAchievements, Achievement[] finishedAchievements) {
            this.startedAchievements = startedAchievements;
            this.finishedAchievements = finishedAchievements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.startedAchievements.Length);
            foreach (var entry in this.startedAchievements) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.finishedAchievements.Length);
            foreach (var entry in this.finishedAchievements) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.startedAchievements = new Achievement[limit];
            for (int i = 0; i < limit; i++) {
                this.startedAchievements[i] = new Achievement();
                this.startedAchievements[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.finishedAchievements = new Achievement[limit];
            for (int i = 0; i < limit; i++) {
                this.finishedAchievements[i] = new Achievement();
                this.finishedAchievements[i].Deserialize(reader);
            }
        }
    }
}