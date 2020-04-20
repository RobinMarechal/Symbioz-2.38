using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage {
        public const ushort Id = 6000;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort objectGenericId;


        public ExchangeCraftResultWithObjectIdMessage() { }

        public ExchangeCraftResultWithObjectIdMessage(sbyte craftResult, ushort objectGenericId)
            : base(craftResult) {
            this.objectGenericId = objectGenericId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.objectGenericId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.objectGenericId = reader.ReadVarUhShort();

            if (this.objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + this.objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
        }
    }
}