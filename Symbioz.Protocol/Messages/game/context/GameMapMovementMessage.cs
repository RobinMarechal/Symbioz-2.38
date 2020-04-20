using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameMapMovementMessage : Message {
        public const ushort Id = 951;

        public override ushort MessageId {
            get { return Id; }
        }

        public short[] keyMovements;
        public double actorId;


        public GameMapMovementMessage() { }

        public GameMapMovementMessage(short[] keyMovements, double actorId) {
            this.keyMovements = keyMovements;
            this.actorId = actorId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.keyMovements.Length);
            foreach (var entry in this.keyMovements) {
                writer.WriteShort(entry);
            }

            writer.WriteDouble(this.actorId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.keyMovements = new short[limit];
            for (int i = 0; i < limit; i++) {
                this.keyMovements[i] = reader.ReadShort();
            }

            this.actorId = reader.ReadDouble();

            if (this.actorId < -9007199254740990 || this.actorId > 9007199254740990)
                throw new Exception("Forbidden value on actorId = " + this.actorId + ", it doesn't respect the following condition : actorId < -9007199254740990 || actorId > 9007199254740990");
        }
    }
}