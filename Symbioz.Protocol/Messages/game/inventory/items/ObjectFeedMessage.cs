using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectFeedMessage : Message {
        public const ushort Id = 6290;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;
        public uint foodUID;
        public uint foodQuantity;


        public ObjectFeedMessage() { }

        public ObjectFeedMessage(uint objectUID, uint foodUID, uint foodQuantity) {
            this.objectUID = objectUID;
            this.foodUID = foodUID;
            this.foodQuantity = foodQuantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
            writer.WriteVarUhInt(this.foodUID);
            writer.WriteVarUhInt(this.foodQuantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
            this.foodUID = reader.ReadVarUhInt();

            if (this.foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + this.foodUID + ", it doesn't respect the following condition : foodUID < 0");
            this.foodQuantity = reader.ReadVarUhInt();

            if (this.foodQuantity < 0)
                throw new Exception("Forbidden value on foodQuantity = " + this.foodQuantity + ", it doesn't respect the following condition : foodQuantity < 0");
        }
    }
}