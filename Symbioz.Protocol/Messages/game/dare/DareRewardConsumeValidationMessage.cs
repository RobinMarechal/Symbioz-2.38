using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareRewardConsumeValidationMessage : Message {
        public const ushort Id = 6675;

        public override ushort MessageId {
            get { return Id; }
        }

        public double dareId;
        public sbyte type;


        public DareRewardConsumeValidationMessage() { }

        public DareRewardConsumeValidationMessage(double dareId, sbyte type) {
            this.dareId = dareId;
            this.type = type;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.dareId);
            writer.WriteSByte(this.type);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dareId = reader.ReadDouble();

            if (this.dareId < 0 || this.dareId > 9007199254740990)
                throw new Exception("Forbidden value on dareId = " + this.dareId + ", it doesn't respect the following condition : dareId < 0 || dareId > 9007199254740990");
            this.type = reader.ReadSByte();

            if (this.type < 0)
                throw new Exception("Forbidden value on type = " + this.type + ", it doesn't respect the following condition : type < 0");
        }
    }
}