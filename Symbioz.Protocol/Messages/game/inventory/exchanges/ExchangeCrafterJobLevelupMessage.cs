using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeCrafterJobLevelupMessage : Message {
        public const ushort Id = 6598;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte crafterJobLevel;


        public ExchangeCrafterJobLevelupMessage() { }

        public ExchangeCrafterJobLevelupMessage(byte crafterJobLevel) {
            this.crafterJobLevel = crafterJobLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.crafterJobLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.crafterJobLevel = reader.ReadByte();

            if (this.crafterJobLevel < 0 || this.crafterJobLevel > 255)
                throw new Exception("Forbidden value on crafterJobLevel = " + this.crafterJobLevel + ", it doesn't respect the following condition : crafterJobLevel < 0 || crafterJobLevel > 255");
        }
    }
}