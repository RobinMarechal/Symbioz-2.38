using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AchievementDetailedListRequestMessage : Message {
        public const ushort Id = 6357;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort categoryId;


        public AchievementDetailedListRequestMessage() { }

        public AchievementDetailedListRequestMessage(ushort categoryId) {
            this.categoryId = categoryId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.categoryId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.categoryId = reader.ReadVarUhShort();

            if (this.categoryId < 0)
                throw new Exception("Forbidden value on categoryId = " + this.categoryId + ", it doesn't respect the following condition : categoryId < 0");
        }
    }
}