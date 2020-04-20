using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SequenceEndMessage : Message {
        public const ushort Id = 956;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort actionId;
        public double authorId;
        public sbyte sequenceType;


        public SequenceEndMessage() { }

        public SequenceEndMessage(ushort actionId, double authorId, sbyte sequenceType) {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.actionId);
            writer.WriteDouble(this.authorId);
            writer.WriteSByte(this.sequenceType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.actionId = reader.ReadVarUhShort();

            if (this.actionId < 0)
                throw new Exception("Forbidden value on actionId = " + this.actionId + ", it doesn't respect the following condition : actionId < 0");
            this.authorId = reader.ReadDouble();

            if (this.authorId < -9007199254740990 || this.authorId > 9007199254740990)
                throw new Exception("Forbidden value on authorId = " + this.authorId + ", it doesn't respect the following condition : authorId < -9007199254740990 || authorId > 9007199254740990");
            this.sequenceType = reader.ReadSByte();
        }
    }
}