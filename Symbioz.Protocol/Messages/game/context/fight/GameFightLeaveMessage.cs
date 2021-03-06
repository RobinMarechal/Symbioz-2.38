using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightLeaveMessage : Message {
        public const ushort Id = 721;

        public override ushort MessageId {
            get { return Id; }
        }

        public double charId;


        public GameFightLeaveMessage() { }

        public GameFightLeaveMessage(double charId) {
            this.charId = charId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.charId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.charId = reader.ReadDouble();

            if (this.charId < -9007199254740990 || this.charId > 9007199254740990)
                throw new Exception("Forbidden value on charId = " + this.charId + ", it doesn't respect the following condition : charId < -9007199254740990 || charId > 9007199254740990");
        }
    }
}