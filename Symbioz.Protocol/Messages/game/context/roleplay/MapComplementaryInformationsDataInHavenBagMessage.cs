using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapComplementaryInformationsDataInHavenBagMessage : MapComplementaryInformationsDataMessage {
        public const ushort Id = 6622;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterMinimalInformations ownerInformations;
        public sbyte theme;
        public sbyte roomId;
        public sbyte maxRoomId;


        public MapComplementaryInformationsDataInHavenBagMessage() { }

        public MapComplementaryInformationsDataInHavenBagMessage(ushort subAreaId,
                                                                 int mapId,
                                                                 HouseInformations[] houses,
                                                                 GameRolePlayActorInformations[] actors,
                                                                 InteractiveElement[] interactiveElements,
                                                                 StatedElement[] statedElements,
                                                                 MapObstacle[] obstacles,
                                                                 FightCommonInformations[] fights,
                                                                 bool hasAggressiveMonsters,
                                                                 CharacterMinimalInformations ownerInformations,
                                                                 sbyte theme,
                                                                 sbyte roomId,
                                                                 sbyte maxRoomId)
            : base(subAreaId, mapId, houses, actors, interactiveElements, statedElements, obstacles, fights, hasAggressiveMonsters) {
            this.ownerInformations = ownerInformations;
            this.theme = theme;
            this.roomId = roomId;
            this.maxRoomId = maxRoomId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.ownerInformations.Serialize(writer);
            writer.WriteSByte(this.theme);
            writer.WriteSByte(this.roomId);
            writer.WriteSByte(this.maxRoomId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.ownerInformations = new CharacterMinimalInformations();
            this.ownerInformations.Deserialize(reader);
            this.theme = reader.ReadSByte();
            this.roomId = reader.ReadSByte();

            if (this.roomId < 0)
                throw new Exception("Forbidden value on roomId = " + this.roomId + ", it doesn't respect the following condition : roomId < 0");
            this.maxRoomId = reader.ReadSByte();

            if (this.maxRoomId < 0)
                throw new Exception("Forbidden value on maxRoomId = " + this.maxRoomId + ", it doesn't respect the following condition : maxRoomId < 0");
        }
    }
}