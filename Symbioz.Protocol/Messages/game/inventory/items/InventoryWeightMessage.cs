using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryWeightMessage : Message {
        public const ushort Id = 3009;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint weight;
        public uint weightMax;


        public InventoryWeightMessage() { }

        public InventoryWeightMessage(uint weight, uint weightMax) {
            this.weight = weight;
            this.weightMax = weightMax;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.weight);
            writer.WriteVarUhInt(this.weightMax);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.weight = reader.ReadVarUhInt();

            if (this.weight < 0)
                throw new Exception("Forbidden value on weight = " + this.weight + ", it doesn't respect the following condition : weight < 0");
            this.weightMax = reader.ReadVarUhInt();

            if (this.weightMax < 0)
                throw new Exception("Forbidden value on weightMax = " + this.weightMax + ", it doesn't respect the following condition : weightMax < 0");
        }
    }
}