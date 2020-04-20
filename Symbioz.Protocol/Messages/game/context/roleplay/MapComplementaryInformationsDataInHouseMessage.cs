using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapComplementaryInformationsDataInHouseMessage : MapComplementaryInformationsDataMessage {
        public const ushort Id = 6130;

        public override ushort MessageId {
            get { return Id; }
        }

        public HouseInformationsInside currentHouse;


        public MapComplementaryInformationsDataInHouseMessage() { }

        public MapComplementaryInformationsDataInHouseMessage(ushort subAreaId,
                                                              int mapId,
                                                              HouseInformations[] houses,
                                                              GameRolePlayActorInformations[] actors,
                                                              InteractiveElement[] interactiveElements,
                                                              StatedElement[] statedElements,
                                                              MapObstacle[] obstacles,
                                                              FightCommonInformations[] fights,
                                                              bool hasAggressiveMonsters,
                                                              HouseInformationsInside currentHouse)
            : base(subAreaId, mapId, houses, actors, interactiveElements, statedElements, obstacles, fights, hasAggressiveMonsters) {
            this.currentHouse = currentHouse;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.currentHouse.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.currentHouse = new HouseInformationsInside();
            this.currentHouse.Deserialize(reader);
        }
    }
}