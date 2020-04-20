using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntRequestMessage : Message {
        public const ushort Id = 6488;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte questLevel;
        public sbyte questType;


        public TreasureHuntRequestMessage() { }

        public TreasureHuntRequestMessage(byte questLevel, sbyte questType) {
            this.questLevel = questLevel;
            this.questType = questType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.questLevel);
            writer.WriteSByte(this.questType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questLevel = reader.ReadByte();

            if (this.questLevel < 1 || this.questLevel > 200)
                throw new Exception("Forbidden value on questLevel = " + this.questLevel + ", it doesn't respect the following condition : questLevel < 1 || questLevel > 200");
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
        }
    }
}