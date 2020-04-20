// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class PrismGeolocalizedInformation : PrismSubareaEmptyInfo {
        public const short Id = 434;

        public override short TypeId {
            get { return Id; }
        }

        public short worldX;
        public short worldY;
        public int mapId;
        public PrismInformation prism;


        public PrismGeolocalizedInformation() { }

        public PrismGeolocalizedInformation(ushort subAreaId, uint allianceId, short worldX, short worldY, int mapId, PrismInformation prism)
            : base(subAreaId, allianceId) {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.prism = prism;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.worldX);
            writer.WriteShort(this.worldY);
            writer.WriteInt(this.mapId);
            writer.WriteShort(this.prism.TypeId);
            this.prism.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.worldX = reader.ReadShort();

            if (this.worldX < -255 || this.worldX > 255)
                throw new Exception("Forbidden value on worldX = " + this.worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            this.worldY = reader.ReadShort();

            if (this.worldY < -255 || this.worldY > 255)
                throw new Exception("Forbidden value on worldY = " + this.worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            this.mapId = reader.ReadInt();
            this.prism = ProtocolTypeManager.GetInstance<PrismInformation>(reader.ReadShort());
            this.prism.Deserialize(reader);
        }
    }
}