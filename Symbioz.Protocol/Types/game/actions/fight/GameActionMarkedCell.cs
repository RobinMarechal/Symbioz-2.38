// Generated on 04/27/2016 01:13:09

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameActionMarkedCell {
        public const short Id = 85;

        public virtual short TypeId {
            get { return Id; }
        }

        public ushort cellId;
        public sbyte zoneSize;
        public int cellColor;
        public sbyte cellsType;


        public GameActionMarkedCell() { }

        public GameActionMarkedCell(ushort cellId, sbyte zoneSize, int cellColor, sbyte cellsType) {
            this.cellId = cellId;
            this.zoneSize = zoneSize;
            this.cellColor = cellColor;
            this.cellsType = cellsType;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.cellId);
            writer.WriteSByte(this.zoneSize);
            writer.WriteInt(this.cellColor);
            writer.WriteSByte(this.cellsType);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.cellId = reader.ReadVarUhShort();

            if (this.cellId < 0 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            this.zoneSize = reader.ReadSByte();
            this.cellColor = reader.ReadInt();
            this.cellsType = reader.ReadSByte();
        }
    }
}