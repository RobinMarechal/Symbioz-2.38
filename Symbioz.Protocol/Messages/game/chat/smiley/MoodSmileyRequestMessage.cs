using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MoodSmileyRequestMessage : Message {
        public const ushort Id = 6192;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort smileyId;


        public MoodSmileyRequestMessage() { }

        public MoodSmileyRequestMessage(ushort smileyId) {
            this.smileyId = smileyId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.smileyId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.smileyId = reader.ReadVarUhShort();

            if (this.smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + this.smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
    }
}