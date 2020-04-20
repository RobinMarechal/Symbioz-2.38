using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntLegendaryRequestMessage : Message {
        public const ushort Id = 6499;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort legendaryId;


        public TreasureHuntLegendaryRequestMessage() { }

        public TreasureHuntLegendaryRequestMessage(ushort legendaryId) {
            this.legendaryId = legendaryId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.legendaryId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.legendaryId = reader.ReadVarUhShort();

            if (this.legendaryId < 0)
                throw new Exception("Forbidden value on legendaryId = " + this.legendaryId + ", it doesn't respect the following condition : legendaryId < 0");
        }
    }
}