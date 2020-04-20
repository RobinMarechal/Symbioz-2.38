using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StatsUpgradeRequestMessage : Message {
        public const ushort Id = 5610;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool useAdditionnal;
        public sbyte statId;
        public ushort boostPoint;


        public StatsUpgradeRequestMessage() { }

        public StatsUpgradeRequestMessage(bool useAdditionnal, sbyte statId, ushort boostPoint) {
            this.useAdditionnal = useAdditionnal;
            this.statId = statId;
            this.boostPoint = boostPoint;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.useAdditionnal);
            writer.WriteSByte(this.statId);
            writer.WriteVarUhShort(this.boostPoint);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.useAdditionnal = reader.ReadBoolean();
            this.statId = reader.ReadSByte();

            if (this.statId < 0)
                throw new Exception("Forbidden value on statId = " + this.statId + ", it doesn't respect the following condition : statId < 0");
            this.boostPoint = reader.ReadVarUhShort();

            if (this.boostPoint < 0)
                throw new Exception("Forbidden value on boostPoint = " + this.boostPoint + ", it doesn't respect the following condition : boostPoint < 0");
        }
    }
}