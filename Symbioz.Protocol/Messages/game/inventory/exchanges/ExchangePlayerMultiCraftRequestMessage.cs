using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage {
        public const ushort Id = 5784;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong target;
        public uint skillId;


        public ExchangePlayerMultiCraftRequestMessage() { }

        public ExchangePlayerMultiCraftRequestMessage(sbyte exchangeType, ulong target, uint skillId)
            : base(exchangeType) {
            this.target = target;
            this.skillId = skillId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.target);
            writer.WriteVarUhInt(this.skillId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.target = reader.ReadVarUhLong();

            if (this.target < 0 || this.target > 9007199254740990)
                throw new Exception("Forbidden value on target = " + this.target + ", it doesn't respect the following condition : target < 0 || target > 9007199254740990");
            this.skillId = reader.ReadVarUhInt();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
        }
    }
}