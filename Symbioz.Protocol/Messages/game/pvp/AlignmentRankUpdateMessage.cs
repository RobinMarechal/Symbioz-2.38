using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AlignmentRankUpdateMessage : Message {
        public const ushort Id = 6058;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte alignmentRank;
        public bool verbose;


        public AlignmentRankUpdateMessage() { }

        public AlignmentRankUpdateMessage(sbyte alignmentRank, bool verbose) {
            this.alignmentRank = alignmentRank;
            this.verbose = verbose;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.alignmentRank);
            writer.WriteBoolean(this.verbose);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.alignmentRank = reader.ReadSByte();

            if (this.alignmentRank < 0)
                throw new Exception("Forbidden value on alignmentRank = " + this.alignmentRank + ", it doesn't respect the following condition : alignmentRank < 0");
            this.verbose = reader.ReadBoolean();
        }
    }
}