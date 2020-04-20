using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightNewRoundMessage : Message {
        public const ushort Id = 6239;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint roundNumber;


        public GameFightNewRoundMessage() { }

        public GameFightNewRoundMessage(uint roundNumber) {
            this.roundNumber = roundNumber;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.roundNumber);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.roundNumber = reader.ReadVarUhInt();

            if (this.roundNumber < 0)
                throw new Exception("Forbidden value on roundNumber = " + this.roundNumber + ", it doesn't respect the following condition : roundNumber < 0");
        }
    }
}