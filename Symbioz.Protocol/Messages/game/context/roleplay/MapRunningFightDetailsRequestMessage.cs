using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapRunningFightDetailsRequestMessage : Message {
        public const ushort Id = 5750;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;


        public MapRunningFightDetailsRequestMessage() { }

        public MapRunningFightDetailsRequestMessage(int fightId) {
            this.fightId = fightId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
        }
    }
}