using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EmotePlayAbstractMessage : Message {
        public const ushort Id = 5690;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte emoteId;
        public double emoteStartTime;


        public EmotePlayAbstractMessage() { }

        public EmotePlayAbstractMessage(byte emoteId, double emoteStartTime) {
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.emoteId);
            writer.WriteDouble(this.emoteStartTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.emoteId = reader.ReadByte();

            if (this.emoteId < 0 || this.emoteId > 255)
                throw new Exception("Forbidden value on emoteId = " + this.emoteId + ", it doesn't respect the following condition : emoteId < 0 || emoteId > 255");
            this.emoteStartTime = reader.ReadDouble();

            if (this.emoteStartTime < -9007199254740990 || this.emoteStartTime > 9007199254740990)
                throw new Exception("Forbidden value on emoteStartTime = " + this.emoteStartTime + ", it doesn't respect the following condition : emoteStartTime < -9007199254740990 || emoteStartTime > 9007199254740990");
        }
    }
}