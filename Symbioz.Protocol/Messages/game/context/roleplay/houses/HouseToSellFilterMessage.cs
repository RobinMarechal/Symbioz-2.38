using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HouseToSellFilterMessage : Message {
        public const ushort Id = 6137;

        public override ushort MessageId {
            get { return Id; }
        }

        public int areaId;
        public sbyte atLeastNbRoom;
        public sbyte atLeastNbChest;
        public ushort skillRequested;
        public uint maxPrice;


        public HouseToSellFilterMessage() { }

        public HouseToSellFilterMessage(int areaId, sbyte atLeastNbRoom, sbyte atLeastNbChest, ushort skillRequested, uint maxPrice) {
            this.areaId = areaId;
            this.atLeastNbRoom = atLeastNbRoom;
            this.atLeastNbChest = atLeastNbChest;
            this.skillRequested = skillRequested;
            this.maxPrice = maxPrice;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.areaId);
            writer.WriteSByte(this.atLeastNbRoom);
            writer.WriteSByte(this.atLeastNbChest);
            writer.WriteVarUhShort(this.skillRequested);
            writer.WriteVarUhInt(this.maxPrice);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.areaId = reader.ReadInt();
            this.atLeastNbRoom = reader.ReadSByte();

            if (this.atLeastNbRoom < 0)
                throw new Exception("Forbidden value on atLeastNbRoom = " + this.atLeastNbRoom + ", it doesn't respect the following condition : atLeastNbRoom < 0");
            this.atLeastNbChest = reader.ReadSByte();

            if (this.atLeastNbChest < 0)
                throw new Exception("Forbidden value on atLeastNbChest = " + this.atLeastNbChest + ", it doesn't respect the following condition : atLeastNbChest < 0");
            this.skillRequested = reader.ReadVarUhShort();

            if (this.skillRequested < 0)
                throw new Exception("Forbidden value on skillRequested = " + this.skillRequested + ", it doesn't respect the following condition : skillRequested < 0");
            this.maxPrice = reader.ReadVarUhInt();

            if (this.maxPrice < 0)
                throw new Exception("Forbidden value on maxPrice = " + this.maxPrice + ", it doesn't respect the following condition : maxPrice < 0");
        }
    }
}