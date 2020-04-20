using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkMulticraftCustomerMessage : Message {
        public const ushort Id = 5817;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint skillId;
        public byte crafterJobLevel;


        public ExchangeStartOkMulticraftCustomerMessage() { }

        public ExchangeStartOkMulticraftCustomerMessage(uint skillId, byte crafterJobLevel) {
            this.skillId = skillId;
            this.crafterJobLevel = crafterJobLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.skillId);
            writer.WriteByte(this.crafterJobLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.skillId = reader.ReadVarUhInt();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
            this.crafterJobLevel = reader.ReadByte();

            if (this.crafterJobLevel < 0 || this.crafterJobLevel > 255)
                throw new Exception("Forbidden value on crafterJobLevel = " + this.crafterJobLevel + ", it doesn't respect the following condition : crafterJobLevel < 0 || crafterJobLevel > 255");
        }
    }
}