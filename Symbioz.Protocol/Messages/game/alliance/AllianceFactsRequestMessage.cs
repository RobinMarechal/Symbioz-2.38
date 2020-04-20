using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceFactsRequestMessage : Message {
        public const ushort Id = 6409;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint allianceId;


        public AllianceFactsRequestMessage() { }

        public AllianceFactsRequestMessage(uint allianceId) {
            this.allianceId = allianceId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.allianceId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceId = reader.ReadVarUhInt();

            if (this.allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + this.allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
    }
}