using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightNewWaveMessage : Message {
        public const ushort Id = 6490;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte id;
        public sbyte teamId;
        public short nbTurnBeforeNextWave;


        public GameFightNewWaveMessage() { }

        public GameFightNewWaveMessage(sbyte id, sbyte teamId, short nbTurnBeforeNextWave) {
            this.id = id;
            this.teamId = teamId;
            this.nbTurnBeforeNextWave = nbTurnBeforeNextWave;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.id);
            writer.WriteSByte(this.teamId);
            writer.WriteShort(this.nbTurnBeforeNextWave);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadSByte();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
            this.teamId = reader.ReadSByte();

            if (this.teamId < 0)
                throw new Exception("Forbidden value on teamId = " + this.teamId + ", it doesn't respect the following condition : teamId < 0");
            this.nbTurnBeforeNextWave = reader.ReadShort();
        }
    }
}