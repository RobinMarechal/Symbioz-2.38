using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyNewGuestMessage : AbstractPartyEventMessage {
        public const ushort Id = 6260;

        public override ushort MessageId {
            get { return Id; }
        }

        public PartyGuestInformations guest;


        public PartyNewGuestMessage() { }

        public PartyNewGuestMessage(uint partyId, PartyGuestInformations guest)
            : base(partyId) {
            this.guest = guest;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.guest.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.guest = new PartyGuestInformations();
            this.guest.Deserialize(reader);
        }
    }
}