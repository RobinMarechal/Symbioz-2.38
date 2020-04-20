using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightStartingMessage : Message {
        public const ushort Id = 700;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte fightType;
        public double attackerId;
        public double defenderId;


        public GameFightStartingMessage() { }

        public GameFightStartingMessage(sbyte fightType, double attackerId, double defenderId) {
            this.fightType = fightType;
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.fightType);
            writer.WriteDouble(this.attackerId);
            writer.WriteDouble(this.defenderId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightType = reader.ReadSByte();

            if (this.fightType < 0)
                throw new Exception("Forbidden value on fightType = " + this.fightType + ", it doesn't respect the following condition : fightType < 0");
            this.attackerId = reader.ReadDouble();

            if (this.attackerId < -9007199254740990 || this.attackerId > 9007199254740990)
                throw new Exception("Forbidden value on attackerId = " + this.attackerId + ", it doesn't respect the following condition : attackerId < -9007199254740990 || attackerId > 9007199254740990");
            this.defenderId = reader.ReadDouble();

            if (this.defenderId < -9007199254740990 || this.defenderId > 9007199254740990)
                throw new Exception("Forbidden value on defenderId = " + this.defenderId + ", it doesn't respect the following condition : defenderId < -9007199254740990 || defenderId > 9007199254740990");
        }
    }
}