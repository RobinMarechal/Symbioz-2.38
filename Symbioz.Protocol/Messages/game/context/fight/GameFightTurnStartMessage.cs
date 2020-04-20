using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightTurnStartMessage : Message {
        public const ushort Id = 714;

        public override ushort MessageId {
            get { return Id; }
        }

        public double id;
        public uint waitTime;


        public GameFightTurnStartMessage() { }

        public GameFightTurnStartMessage(double id, uint waitTime) {
            this.id = id;
            this.waitTime = waitTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
            writer.WriteVarUhInt(this.waitTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            this.waitTime = reader.ReadVarUhInt();

            if (this.waitTime < 0)
                throw new Exception("Forbidden value on waitTime = " + this.waitTime + ", it doesn't respect the following condition : waitTime < 0");
        }
    }
}