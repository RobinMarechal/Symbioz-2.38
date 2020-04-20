using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolFightPreparationUpdateMessage : Message {
        public const ushort Id = 6586;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte idolSource;
        public Idol[] idols;


        public IdolFightPreparationUpdateMessage() { }

        public IdolFightPreparationUpdateMessage(sbyte idolSource, Idol[] idols) {
            this.idolSource = idolSource;
            this.idols = idols;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.idolSource);
            writer.WriteUShort((ushort) this.idols.Length);
            foreach (var entry in this.idols) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.idolSource = reader.ReadSByte();

            if (this.idolSource < 0)
                throw new Exception("Forbidden value on idolSource = " + this.idolSource + ", it doesn't respect the following condition : idolSource < 0");
            var limit = reader.ReadUShort();
            this.idols = new Idol[limit];
            for (int i = 0; i < limit; i++) {
                this.idols[i] = ProtocolTypeManager.GetInstance<Idol>(reader.ReadShort());
                this.idols[i].Deserialize(reader);
            }
        }
    }
}