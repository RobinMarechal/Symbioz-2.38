using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MoodSmileyResultMessage : Message {
        public const ushort Id = 6196;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte resultCode;
        public ushort smileyId;


        public MoodSmileyResultMessage() { }

        public MoodSmileyResultMessage(sbyte resultCode, ushort smileyId) {
            this.resultCode = resultCode;
            this.smileyId = smileyId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.resultCode);
            writer.WriteVarUhShort(this.smileyId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.resultCode = reader.ReadSByte();

            if (this.resultCode < 0)
                throw new Exception("Forbidden value on resultCode = " + this.resultCode + ", it doesn't respect the following condition : resultCode < 0");
            this.smileyId = reader.ReadVarUhShort();

            if (this.smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + this.smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
    }
}