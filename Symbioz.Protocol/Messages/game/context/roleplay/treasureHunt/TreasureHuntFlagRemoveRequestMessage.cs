using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntFlagRemoveRequestMessage : Message {
        public const ushort Id = 6510;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte questType;
        public sbyte index;


        public TreasureHuntFlagRemoveRequestMessage() { }

        public TreasureHuntFlagRemoveRequestMessage(sbyte questType, sbyte index) {
            this.questType = questType;
            this.index = index;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.questType);
            writer.WriteSByte(this.index);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
            this.index = reader.ReadSByte();

            if (this.index < 0)
                throw new Exception("Forbidden value on index = " + this.index + ", it doesn't respect the following condition : index < 0");
        }
    }
}