using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightOptionStateUpdateMessage : Message {
        public const ushort Id = 5927;

        public override ushort MessageId {
            get { return Id; }
        }

        public short fightId;
        public sbyte teamId;
        public sbyte option;
        public bool state;


        public GameFightOptionStateUpdateMessage() { }

        public GameFightOptionStateUpdateMessage(short fightId, sbyte teamId, sbyte option, bool state) {
            this.fightId = fightId;
            this.teamId = teamId;
            this.option = option;
            this.state = state;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.fightId);
            writer.WriteSByte(this.teamId);
            writer.WriteSByte(this.option);
            writer.WriteBoolean(this.state);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadShort();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.teamId = reader.ReadSByte();

            if (this.teamId < 0)
                throw new Exception("Forbidden value on teamId = " + this.teamId + ", it doesn't respect the following condition : teamId < 0");
            this.option = reader.ReadSByte();

            if (this.option < 0)
                throw new Exception("Forbidden value on option = " + this.option + ", it doesn't respect the following condition : option < 0");
            this.state = reader.ReadBoolean();
        }
    }
}