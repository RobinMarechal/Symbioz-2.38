using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeOkMultiCraftMessage : Message {
        public const ushort Id = 5768;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong initiatorId;
        public ulong otherId;
        public sbyte role;


        public ExchangeOkMultiCraftMessage() { }

        public ExchangeOkMultiCraftMessage(ulong initiatorId, ulong otherId, sbyte role) {
            this.initiatorId = initiatorId;
            this.otherId = otherId;
            this.role = role;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.initiatorId);
            writer.WriteVarUhLong(this.otherId);
            writer.WriteSByte(this.role);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.initiatorId = reader.ReadVarUhLong();

            if (this.initiatorId < 0 || this.initiatorId > 9007199254740990)
                throw new Exception("Forbidden value on initiatorId = " + this.initiatorId + ", it doesn't respect the following condition : initiatorId < 0 || initiatorId > 9007199254740990");
            this.otherId = reader.ReadVarUhLong();

            if (this.otherId < 0 || this.otherId > 9007199254740990)
                throw new Exception("Forbidden value on otherId = " + this.otherId + ", it doesn't respect the following condition : otherId < 0 || otherId > 9007199254740990");
            this.role = reader.ReadSByte();
        }
    }
}