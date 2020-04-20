using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InviteInHavenBagClosedMessage : Message {
        public const ushort Id = 6645;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterMinimalInformations hostInformations;


        public InviteInHavenBagClosedMessage() { }

        public InviteInHavenBagClosedMessage(CharacterMinimalInformations hostInformations) {
            this.hostInformations = hostInformations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.hostInformations.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.hostInformations = new CharacterMinimalInformations();
            this.hostInformations.Deserialize(reader);
        }
    }
}