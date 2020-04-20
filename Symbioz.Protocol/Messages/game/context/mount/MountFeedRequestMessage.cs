using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountFeedRequestMessage : Message {
        public const ushort Id = 6189;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint mountUid;
        public sbyte mountLocation;
        public uint mountFoodUid;
        public uint quantity;


        public MountFeedRequestMessage() { }

        public MountFeedRequestMessage(uint mountUid, sbyte mountLocation, uint mountFoodUid, uint quantity) {
            this.mountUid = mountUid;
            this.mountLocation = mountLocation;
            this.mountFoodUid = mountFoodUid;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.mountUid);
            writer.WriteSByte(this.mountLocation);
            writer.WriteVarUhInt(this.mountFoodUid);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mountUid = reader.ReadVarUhInt();

            if (this.mountUid < 0)
                throw new Exception("Forbidden value on mountUid = " + this.mountUid + ", it doesn't respect the following condition : mountUid < 0");
            this.mountLocation = reader.ReadSByte();
            this.mountFoodUid = reader.ReadVarUhInt();

            if (this.mountFoodUid < 0)
                throw new Exception("Forbidden value on mountFoodUid = " + this.mountFoodUid + ", it doesn't respect the following condition : mountFoodUid < 0");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}