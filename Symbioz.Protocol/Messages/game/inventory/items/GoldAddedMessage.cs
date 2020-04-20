using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GoldAddedMessage : Message {
        public const ushort Id = 6030;

        public override ushort MessageId {
            get { return Id; }
        }

        public GoldItem gold;


        public GoldAddedMessage() { }

        public GoldAddedMessage(GoldItem gold) {
            this.gold = gold;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.gold.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.gold = new GoldItem();
            this.gold.Deserialize(reader);
        }
    }
}