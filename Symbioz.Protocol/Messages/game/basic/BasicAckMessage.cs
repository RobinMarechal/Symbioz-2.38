using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicAckMessage : Message {
        public const ushort Id = 6362;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint seq;
        public ushort lastPacketId;


        public BasicAckMessage() { }

        public BasicAckMessage(uint seq, ushort lastPacketId) {
            this.seq = seq;
            this.lastPacketId = lastPacketId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.seq);
            writer.WriteVarUhShort(this.lastPacketId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.seq = reader.ReadVarUhInt();

            if (this.seq < 0)
                throw new Exception("Forbidden value on seq = " + this.seq + ", it doesn't respect the following condition : seq < 0");
            this.lastPacketId = reader.ReadVarUhShort();

            if (this.lastPacketId < 0)
                throw new Exception("Forbidden value on lastPacketId = " + this.lastPacketId + ", it doesn't respect the following condition : lastPacketId < 0");
        }
    }
}