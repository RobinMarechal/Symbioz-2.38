using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBuyMessage : Message {
        public const ushort Id = 5774;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectToBuyId;
        public uint quantity;


        public ExchangeBuyMessage() { }

        public ExchangeBuyMessage(uint objectToBuyId, uint quantity) {
            this.objectToBuyId = objectToBuyId;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectToBuyId);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectToBuyId = reader.ReadVarUhInt();

            if (this.objectToBuyId < 0)
                throw new Exception("Forbidden value on objectToBuyId = " + this.objectToBuyId + ", it doesn't respect the following condition : objectToBuyId < 0");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}