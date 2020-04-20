using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterReplayWithRemodelRequestMessage : CharacterReplayRequestMessage {
        public const ushort Id = 6551;

        public override ushort MessageId {
            get { return Id; }
        }

        public RemodelingInformation remodel;


        public CharacterReplayWithRemodelRequestMessage() { }

        public CharacterReplayWithRemodelRequestMessage(ulong characterId, RemodelingInformation remodel)
            : base(characterId) {
            this.remodel = remodel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.remodel.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.remodel = new RemodelingInformation();
            this.remodel.Deserialize(reader);
        }
    }
}