using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapObstacleUpdateMessage : Message {
        public const ushort Id = 6051;

        public override ushort MessageId {
            get { return Id; }
        }

        public MapObstacle[] obstacles;


        public MapObstacleUpdateMessage() { }

        public MapObstacleUpdateMessage(MapObstacle[] obstacles) {
            this.obstacles = obstacles;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.obstacles.Length);
            foreach (var entry in this.obstacles) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.obstacles = new MapObstacle[limit];
            for (int i = 0; i < limit; i++) {
                this.obstacles[i] = new MapObstacle();
                this.obstacles[i].Deserialize(reader);
            }
        }
    }
}