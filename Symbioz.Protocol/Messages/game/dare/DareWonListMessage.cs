using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareWonListMessage : Message {
        public const ushort Id = 6682;

        public override ushort MessageId {
            get { return Id; }
        }

        public double[] dareId;


        public DareWonListMessage() { }

        public DareWonListMessage(double[] dareId) {
            this.dareId = dareId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.dareId.Length);
            foreach (var entry in this.dareId) {
                writer.WriteDouble(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.dareId = new double[limit];
            for (int i = 0; i < limit; i++) {
                this.dareId[i] = reader.ReadDouble();
            }
        }
    }
}