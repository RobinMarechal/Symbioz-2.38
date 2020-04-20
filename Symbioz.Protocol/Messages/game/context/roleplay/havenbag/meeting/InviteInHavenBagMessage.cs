using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InviteInHavenBagMessage : Message {
        public const ushort Id = 6642;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterMinimalInformations guestInformations;
        public bool accept;


        public InviteInHavenBagMessage() { }

        public InviteInHavenBagMessage(CharacterMinimalInformations guestInformations, bool accept) {
            this.guestInformations = guestInformations;
            this.accept = accept;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.guestInformations.Serialize(writer);
            writer.WriteBoolean(this.accept);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guestInformations = new CharacterMinimalInformations();
            this.guestInformations.Deserialize(reader);
            this.accept = reader.ReadBoolean();
        }
    }
}