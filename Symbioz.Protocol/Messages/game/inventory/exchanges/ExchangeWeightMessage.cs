using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeWeightMessage : Message {
        public const ushort Id = 5793;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint currentWeight;
        public uint maxWeight;


        public ExchangeWeightMessage() { }

        public ExchangeWeightMessage(uint currentWeight, uint maxWeight) {
            this.currentWeight = currentWeight;
            this.maxWeight = maxWeight;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.currentWeight);
            writer.WriteVarUhInt(this.maxWeight);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.currentWeight = reader.ReadVarUhInt();

            if (this.currentWeight < 0)
                throw new Exception("Forbidden value on currentWeight = " + this.currentWeight + ", it doesn't respect the following condition : currentWeight < 0");
            this.maxWeight = reader.ReadVarUhInt();

            if (this.maxWeight < 0)
                throw new Exception("Forbidden value on maxWeight = " + this.maxWeight + ", it doesn't respect the following condition : maxWeight < 0");
        }
    }
}