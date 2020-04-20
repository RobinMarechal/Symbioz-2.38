using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapComplementaryInformationsDataMessage : Message {
        public const ushort Id = 226;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;
        public int mapId;
        public HouseInformations[] houses;
        public GameRolePlayActorInformations[] actors;
        public InteractiveElement[] interactiveElements;
        public StatedElement[] statedElements;
        public MapObstacle[] obstacles;
        public FightCommonInformations[] fights;
        public bool hasAggressiveMonsters;


        public MapComplementaryInformationsDataMessage() { }

        public MapComplementaryInformationsDataMessage(ushort subAreaId,
                                                       int mapId,
                                                       HouseInformations[] houses,
                                                       GameRolePlayActorInformations[] actors,
                                                       InteractiveElement[] interactiveElements,
                                                       StatedElement[] statedElements,
                                                       MapObstacle[] obstacles,
                                                       FightCommonInformations[] fights,
                                                       bool hasAggressiveMonsters) {
            this.subAreaId = subAreaId;
            this.mapId = mapId;
            this.houses = houses;
            this.actors = actors;
            this.interactiveElements = interactiveElements;
            this.statedElements = statedElements;
            this.obstacles = obstacles;
            this.fights = fights;
            this.hasAggressiveMonsters = hasAggressiveMonsters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteInt(this.mapId);
            writer.WriteUShort((ushort) this.houses.Length);
            foreach (var entry in this.houses) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.actors.Length);
            foreach (var entry in this.actors) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.interactiveElements.Length);
            foreach (var entry in this.interactiveElements) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.statedElements.Length);
            foreach (var entry in this.statedElements) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.obstacles.Length);
            foreach (var entry in this.obstacles) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.fights.Length);
            foreach (var entry in this.fights) {
                entry.Serialize(writer);
            }

            writer.WriteBoolean(this.hasAggressiveMonsters);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.mapId = reader.ReadInt();

            if (this.mapId < 0)
                throw new Exception("Forbidden value on mapId = " + this.mapId + ", it doesn't respect the following condition : mapId < 0");
            var limit = reader.ReadUShort();
            this.houses = new HouseInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.houses[i] = ProtocolTypeManager.GetInstance<HouseInformations>(reader.ReadShort());
                this.houses[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.actors = new GameRolePlayActorInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.actors[i] = ProtocolTypeManager.GetInstance<GameRolePlayActorInformations>(reader.ReadShort());
                this.actors[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.interactiveElements = new InteractiveElement[limit];
            for (int i = 0; i < limit; i++) {
                this.interactiveElements[i] = ProtocolTypeManager.GetInstance<InteractiveElement>(reader.ReadShort());
                this.interactiveElements[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.statedElements = new StatedElement[limit];
            for (int i = 0; i < limit; i++) {
                this.statedElements[i] = new StatedElement();
                this.statedElements[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.obstacles = new MapObstacle[limit];
            for (int i = 0; i < limit; i++) {
                this.obstacles[i] = new MapObstacle();
                this.obstacles[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.fights = new FightCommonInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.fights[i] = new FightCommonInformations();
                this.fights[i].Deserialize(reader);
            }

            this.hasAggressiveMonsters = reader.ReadBoolean();
        }
    }
}