using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntFinishedMessage : Message {
        public const ushort Id = 6483;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte questType;


        public TreasureHuntFinishedMessage() { }

        public TreasureHuntFinishedMessage(sbyte questType) {
            this.questType = questType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.questType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
        }
    }
}