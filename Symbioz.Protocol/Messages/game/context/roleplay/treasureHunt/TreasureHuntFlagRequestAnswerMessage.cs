using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntFlagRequestAnswerMessage : Message {
        public const ushort Id = 6507;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte questType;
        public sbyte result;
        public sbyte index;


        public TreasureHuntFlagRequestAnswerMessage() { }

        public TreasureHuntFlagRequestAnswerMessage(sbyte questType, sbyte result, sbyte index) {
            this.questType = questType;
            this.result = result;
            this.index = index;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.questType);
            writer.WriteSByte(this.result);
            writer.WriteSByte(this.index);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
            this.result = reader.ReadSByte();

            if (this.result < 0)
                throw new Exception("Forbidden value on result = " + this.result + ", it doesn't respect the following condition : result < 0");
            this.index = reader.ReadSByte();

            if (this.index < 0)
                throw new Exception("Forbidden value on index = " + this.index + ", it doesn't respect the following condition : index < 0");
        }
    }
}