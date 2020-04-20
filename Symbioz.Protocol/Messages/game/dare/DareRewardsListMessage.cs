using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareRewardsListMessage : Message {
        public const ushort Id = 6677;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareReward[] rewards;


        public DareRewardsListMessage() { }

        public DareRewardsListMessage(DareReward[] rewards) {
            this.rewards = rewards;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.rewards.Length);
            foreach (var entry in this.rewards) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.rewards = new DareReward[limit];
            for (int i = 0; i < limit; i++) {
                this.rewards[i] = new DareReward();
                this.rewards[i].Deserialize(reader);
            }
        }
    }
}