using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicLatencyStatsMessage : Message {
        public const ushort Id = 5663;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort latency;
        public ushort sampleCount;
        public ushort max;


        public BasicLatencyStatsMessage() { }

        public BasicLatencyStatsMessage(ushort latency, ushort sampleCount, ushort max) {
            this.latency = latency;
            this.sampleCount = sampleCount;
            this.max = max;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort(this.latency);
            writer.WriteVarUhShort(this.sampleCount);
            writer.WriteVarUhShort(this.max);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.latency = reader.ReadUShort();

            if (this.latency < 0 || this.latency > 65535)
                throw new Exception("Forbidden value on latency = " + this.latency + ", it doesn't respect the following condition : latency < 0 || latency > 65535");
            this.sampleCount = reader.ReadVarUhShort();

            if (this.sampleCount < 0)
                throw new Exception("Forbidden value on sampleCount = " + this.sampleCount + ", it doesn't respect the following condition : sampleCount < 0");
            this.max = reader.ReadVarUhShort();

            if (this.max < 0)
                throw new Exception("Forbidden value on max = " + this.max + ", it doesn't respect the following condition : max < 0");
        }
    }
}