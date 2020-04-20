using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameEntitiesDispositionMessage : Message {
        public const ushort Id = 5696;

        public override ushort MessageId {
            get { return Id; }
        }

        public IdentifiedEntityDispositionInformations[] dispositions;


        public GameEntitiesDispositionMessage() { }

        public GameEntitiesDispositionMessage(IdentifiedEntityDispositionInformations[] dispositions) {
            this.dispositions = dispositions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.dispositions.Length);
            foreach (var entry in this.dispositions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.dispositions = new IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.dispositions[i] = new IdentifiedEntityDispositionInformations();
                this.dispositions[i].Deserialize(reader);
            }
        }
    }
}