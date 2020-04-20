using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectQuantityMessage : Message {
        public const ushort Id = 3023;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;
        public uint quantity;


        public ObjectQuantityMessage() { }

        public ObjectQuantityMessage(uint objectUID, uint quantity) {
            this.objectUID = objectUID;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}