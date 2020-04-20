using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareRewardWonMessage : Message {
        public const ushort Id = 6678;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareReward reward;


        public DareRewardWonMessage() { }

        public DareRewardWonMessage(DareReward reward) {
            this.reward = reward;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.reward.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.reward = new DareReward();
            this.reward.Deserialize(reader);
        }
    }
}