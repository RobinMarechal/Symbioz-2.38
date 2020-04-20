using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobBookSubscriptionMessage : Message {
        public const ushort Id = 6593;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobBookSubscription[] subscriptions;


        public JobBookSubscriptionMessage() { }

        public JobBookSubscriptionMessage(JobBookSubscription[] subscriptions) {
            this.subscriptions = subscriptions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.subscriptions.Length);
            foreach (var entry in this.subscriptions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.subscriptions = new JobBookSubscription[limit];
            for (int i = 0; i < limit; i++) {
                this.subscriptions[i] = new JobBookSubscription();
                this.subscriptions[i].Deserialize(reader);
            }
        }
    }
}