// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HumanOptionEmote : HumanOption {
        public const short Id = 407;

        public override short TypeId {
            get { return Id; }
        }

        public byte emoteId;
        public double emoteStartTime;


        public HumanOptionEmote() { }

        public HumanOptionEmote(byte emoteId, double emoteStartTime) {
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteByte(this.emoteId);
            writer.WriteDouble(this.emoteStartTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.emoteId = reader.ReadByte();

            if (this.emoteId < 0 || this.emoteId > 255)
                throw new Exception("Forbidden value on emoteId = " + this.emoteId + ", it doesn't respect the following condition : emoteId < 0 || emoteId > 255");
            this.emoteStartTime = reader.ReadDouble();

            if (this.emoteStartTime < -9007199254740990 || this.emoteStartTime > 9007199254740990)
                throw new Exception("Forbidden value on emoteStartTime = " + this.emoteStartTime + ", it doesn't respect the following condition : emoteStartTime < -9007199254740990 || emoteStartTime > 9007199254740990");
        }
    }
}