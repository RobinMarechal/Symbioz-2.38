using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextRemoveMultipleElementsMessage : Message {
        public const ushort Id = 252;

        public override ushort MessageId {
            get { return Id; }
        }

        public double[] id;


        public GameContextRemoveMultipleElementsMessage() { }

        public GameContextRemoveMultipleElementsMessage(double[] id) {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.id.Length);
            foreach (var entry in this.id) {
                writer.WriteDouble(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.id = new double[limit];
            for (int i = 0; i < limit; i++) {
                this.id[i] = reader.ReadDouble();
            }
        }
    }
}