using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorStateUpdateMessage : Message {
        public const ushort Id = 6455;

        public override ushort MessageId {
            get { return Id; }
        }

        public int uniqueId;
        public sbyte state;


        public TaxCollectorStateUpdateMessage() { }

        public TaxCollectorStateUpdateMessage(int uniqueId, sbyte state) {
            this.uniqueId = uniqueId;
            this.state = state;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.uniqueId);
            writer.WriteSByte(this.state);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.uniqueId = reader.ReadInt();
            this.state = reader.ReadSByte();
        }
    }
}