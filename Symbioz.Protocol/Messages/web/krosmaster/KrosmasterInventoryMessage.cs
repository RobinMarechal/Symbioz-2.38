using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class KrosmasterInventoryMessage : Message {
        public const ushort Id = 6350;

        public override ushort MessageId {
            get { return Id; }
        }

        public KrosmasterFigure[] figures;


        public KrosmasterInventoryMessage() { }

        public KrosmasterInventoryMessage(KrosmasterFigure[] figures) {
            this.figures = figures;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.figures.Length);
            foreach (var entry in this.figures) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.figures = new KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++) {
                this.figures[i] = new KrosmasterFigure();
                this.figures[i].Deserialize(reader);
            }
        }
    }
}