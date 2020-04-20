using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameMapChangeOrientationsMessage : Message {
        public const ushort Id = 6155;

        public override ushort MessageId {
            get { return Id; }
        }

        public ActorOrientation[] orientations;


        public GameMapChangeOrientationsMessage() { }

        public GameMapChangeOrientationsMessage(ActorOrientation[] orientations) {
            this.orientations = orientations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.orientations.Length);
            foreach (var entry in this.orientations) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.orientations = new ActorOrientation[limit];
            for (int i = 0; i < limit; i++) {
                this.orientations[i] = new ActorOrientation();
                this.orientations[i].Deserialize(reader);
            }
        }
    }
}