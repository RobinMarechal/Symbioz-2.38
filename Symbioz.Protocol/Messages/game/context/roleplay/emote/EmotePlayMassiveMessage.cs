using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EmotePlayMassiveMessage : EmotePlayAbstractMessage {
        public const ushort Id = 5691;

        public override ushort MessageId {
            get { return Id; }
        }

        public double[] actorIds;


        public EmotePlayMassiveMessage() { }

        public EmotePlayMassiveMessage(byte emoteId, double emoteStartTime, double[] actorIds)
            : base(emoteId, emoteStartTime) {
            this.actorIds = actorIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.actorIds.Length);
            foreach (var entry in this.actorIds) {
                writer.WriteDouble(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.actorIds = new double[limit];
            for (int i = 0; i < limit; i++) {
                this.actorIds[i] = reader.ReadDouble();
            }
        }
    }
}