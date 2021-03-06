using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EmoteRemoveMessage : Message {
        public const ushort Id = 5687;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte emoteId;


        public EmoteRemoveMessage() { }

        public EmoteRemoveMessage(byte emoteId) {
            this.emoteId = emoteId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.emoteId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.emoteId = reader.ReadByte();

            if (this.emoteId < 0 || this.emoteId > 255)
                throw new Exception("Forbidden value on emoteId = " + this.emoteId + ", it doesn't respect the following condition : emoteId < 0 || emoteId > 255");
        }
    }
}