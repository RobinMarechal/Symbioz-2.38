using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeOnHumanVendorRequestMessage : Message {
        public const ushort Id = 5772;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong humanVendorId;
        public ushort humanVendorCell;


        public ExchangeOnHumanVendorRequestMessage() { }

        public ExchangeOnHumanVendorRequestMessage(ulong humanVendorId, ushort humanVendorCell) {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.humanVendorId);
            writer.WriteVarUhShort(this.humanVendorCell);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.humanVendorId = reader.ReadVarUhLong();

            if (this.humanVendorId < 0 || this.humanVendorId > 9007199254740990)
                throw new Exception("Forbidden value on humanVendorId = " + this.humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0 || humanVendorId > 9007199254740990");
            this.humanVendorCell = reader.ReadVarUhShort();

            if (this.humanVendorCell < 0 || this.humanVendorCell > 559)
                throw new Exception("Forbidden value on humanVendorCell = " + this.humanVendorCell + ", it doesn't respect the following condition : humanVendorCell < 0 || humanVendorCell > 559");
        }
    }
}