using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StatsUpgradeResultMessage : Message {
        public const ushort Id = 5609;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte result;
        public ushort nbCharacBoost;


        public StatsUpgradeResultMessage() { }

        public StatsUpgradeResultMessage(sbyte result, ushort nbCharacBoost) {
            this.result = result;
            this.nbCharacBoost = nbCharacBoost;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.result);
            writer.WriteVarUhShort(this.nbCharacBoost);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.result = reader.ReadSByte();
            this.nbCharacBoost = reader.ReadVarUhShort();

            if (this.nbCharacBoost < 0)
                throw new Exception("Forbidden value on nbCharacBoost = " + this.nbCharacBoost + ", it doesn't respect the following condition : nbCharacBoost < 0");
        }
    }
}