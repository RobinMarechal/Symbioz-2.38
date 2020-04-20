using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeReplyTaxVendorMessage : Message {
        public const ushort Id = 5787;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectValue;
        public uint totalTaxValue;


        public ExchangeReplyTaxVendorMessage() { }

        public ExchangeReplyTaxVendorMessage(uint objectValue, uint totalTaxValue) {
            this.objectValue = objectValue;
            this.totalTaxValue = totalTaxValue;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectValue);
            writer.WriteVarUhInt(this.totalTaxValue);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectValue = reader.ReadVarUhInt();

            if (this.objectValue < 0)
                throw new Exception("Forbidden value on objectValue = " + this.objectValue + ", it doesn't respect the following condition : objectValue < 0");
            this.totalTaxValue = reader.ReadVarUhInt();

            if (this.totalTaxValue < 0)
                throw new Exception("Forbidden value on totalTaxValue = " + this.totalTaxValue + ", it doesn't respect the following condition : totalTaxValue < 0");
        }
    }
}