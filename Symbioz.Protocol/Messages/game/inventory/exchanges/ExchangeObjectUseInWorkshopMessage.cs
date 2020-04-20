using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeObjectUseInWorkshopMessage : Message {
        public const ushort Id = 6004;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;
        public int quantity;


        public ExchangeObjectUseInWorkshopMessage() { }

        public ExchangeObjectUseInWorkshopMessage(uint objectUID, int quantity) {
            this.objectUID = objectUID;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
            writer.WriteVarInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
            this.quantity = reader.ReadVarInt();
        }
    }
}