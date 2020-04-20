using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightUpdateTeamMessage : Message {
        public const ushort Id = 5572;

        public override ushort MessageId {
            get { return Id; }
        }

        public short fightId;
        public FightTeamInformations team;


        public GameFightUpdateTeamMessage() { }

        public GameFightUpdateTeamMessage(short fightId, FightTeamInformations team) {
            this.fightId = fightId;
            this.team = team;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.fightId);
            this.team.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadShort();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.team = new FightTeamInformations();
            this.team.Deserialize(reader);
        }
    }
}