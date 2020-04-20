using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkMulticraftCrafterMessage : Message {
        public const ushort Id = 5818;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint skillId;


        public ExchangeStartOkMulticraftCrafterMessage() { }

        public ExchangeStartOkMulticraftCrafterMessage(uint skillId) {
            this.skillId = skillId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.skillId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.skillId = reader.ReadVarUhInt();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
        }
    }
}