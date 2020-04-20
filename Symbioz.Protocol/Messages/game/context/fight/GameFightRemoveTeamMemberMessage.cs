using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightRemoveTeamMemberMessage : Message {
        public const ushort Id = 711;

        public override ushort MessageId {
            get { return Id; }
        }

        public short fightId;
        public sbyte teamId;
        public double charId;


        public GameFightRemoveTeamMemberMessage() { }

        public GameFightRemoveTeamMemberMessage(short fightId, sbyte teamId, double charId) {
            this.fightId = fightId;
            this.teamId = teamId;
            this.charId = charId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.fightId);
            writer.WriteSByte(this.teamId);
            writer.WriteDouble(this.charId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadShort();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.teamId = reader.ReadSByte();

            if (this.teamId < 0)
                throw new Exception("Forbidden value on teamId = " + this.teamId + ", it doesn't respect the following condition : teamId < 0");
            this.charId = reader.ReadDouble();

            if (this.charId < -9007199254740990 || this.charId > 9007199254740990)
                throw new Exception("Forbidden value on charId = " + this.charId + ", it doesn't respect the following condition : charId < -9007199254740990 || charId > 9007199254740990");
        }
    }
}