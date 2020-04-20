using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameEntityDispositionMessage : Message {
        public const ushort Id = 5693;

        public override ushort MessageId {
            get { return Id; }
        }

        public IdentifiedEntityDispositionInformations disposition;


        public GameEntityDispositionMessage() { }

        public GameEntityDispositionMessage(IdentifiedEntityDispositionInformations disposition) {
            this.disposition = disposition;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.disposition.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.disposition = new IdentifiedEntityDispositionInformations();
            this.disposition.Deserialize(reader);
        }
    }
}