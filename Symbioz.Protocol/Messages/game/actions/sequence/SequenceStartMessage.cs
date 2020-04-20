using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SequenceStartMessage : Message {
        public const ushort Id = 955;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte sequenceType;
        public double authorId;


        public SequenceStartMessage() { }

        public SequenceStartMessage(sbyte sequenceType, double authorId) {
            this.sequenceType = sequenceType;
            this.authorId = authorId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.sequenceType);
            writer.WriteDouble(this.authorId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.sequenceType = reader.ReadSByte();
            this.authorId = reader.ReadDouble();

            if (this.authorId < -9007199254740990 || this.authorId > 9007199254740990)
                throw new Exception("Forbidden value on authorId = " + this.authorId + ", it doesn't respect the following condition : authorId < -9007199254740990 || authorId > 9007199254740990");
        }
    }
}