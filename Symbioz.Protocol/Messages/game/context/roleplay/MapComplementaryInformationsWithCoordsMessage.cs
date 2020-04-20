using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapComplementaryInformationsWithCoordsMessage : MapComplementaryInformationsDataMessage {
        public const ushort Id = 6268;

        public override ushort MessageId {
            get { return Id; }
        }

        public short worldX;
        public short worldY;


        public MapComplementaryInformationsWithCoordsMessage() { }

        public MapComplementaryInformationsWithCoordsMessage(ushort subAreaId,
                                                             int mapId,
                                                             HouseInformations[] houses,
                                                             GameRolePlayActorInformations[] actors,
                                                             InteractiveElement[] interactiveElements,
                                                             StatedElement[] statedElements,
                                                             MapObstacle[] obstacles,
                                                             FightCommonInformations[] fights,
                                                             bool hasAggressiveMonsters,
                                                             short worldX,
                                                             short worldY)
            : base(subAreaId, mapId, houses, actors, interactiveElements, statedElements, obstacles, fights, hasAggressiveMonsters) {
            this.worldX = worldX;
            this.worldY = worldY;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.worldX);
            writer.WriteShort(this.worldY);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.worldX = reader.ReadShort();

            if (this.worldX < -255 || this.worldX > 255)
                throw new Exception("Forbidden value on worldX = " + this.worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            this.worldY = reader.ReadShort();

            if (this.worldY < -255 || this.worldY > 255)
                throw new Exception("Forbidden value on worldY = " + this.worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
        }
    }
}