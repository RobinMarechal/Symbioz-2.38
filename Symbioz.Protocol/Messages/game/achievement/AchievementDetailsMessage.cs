using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AchievementDetailsMessage : Message {
        public const ushort Id = 6378;

        public override ushort MessageId {
            get { return Id; }
        }

        public Achievement achievement;


        public AchievementDetailsMessage() { }

        public AchievementDetailsMessage(Achievement achievement) {
            this.achievement = achievement;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.achievement.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.achievement = new Achievement();
            this.achievement.Deserialize(reader);
        }
    }
}