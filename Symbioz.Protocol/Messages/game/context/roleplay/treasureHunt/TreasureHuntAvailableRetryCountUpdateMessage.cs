using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntAvailableRetryCountUpdateMessage : Message {
        public const ushort Id = 6491;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte questType;
        public int availableRetryCount;


        public TreasureHuntAvailableRetryCountUpdateMessage() { }

        public TreasureHuntAvailableRetryCountUpdateMessage(sbyte questType, int availableRetryCount) {
            this.questType = questType;
            this.availableRetryCount = availableRetryCount;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.questType);
            writer.WriteInt(this.availableRetryCount);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
            this.availableRetryCount = reader.ReadInt();
        }
    }
}