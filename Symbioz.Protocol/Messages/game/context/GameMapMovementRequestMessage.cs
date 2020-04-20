using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameMapMovementRequestMessage : Message {
        public const ushort Id = 950;

        public override ushort MessageId {
            get { return Id; }
        }

        public short[] keyMovements;
        public int mapId;


        public GameMapMovementRequestMessage() { }

        public GameMapMovementRequestMessage(short[] keyMovements, int mapId) {
            this.keyMovements = keyMovements;
            this.mapId = mapId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.keyMovements.Length);
            foreach (var entry in this.keyMovements) {
                writer.WriteShort(entry);
            }

            writer.WriteInt(this.mapId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.keyMovements = new short[limit];
            for (int i = 0; i < limit; i++) {
                this.keyMovements[i] = reader.ReadShort();
            }

            this.mapId = reader.ReadInt();

            if (this.mapId < 0)
                throw new Exception("Forbidden value on mapId = " + this.mapId + ", it doesn't respect the following condition : mapId < 0");
        }
    }
}