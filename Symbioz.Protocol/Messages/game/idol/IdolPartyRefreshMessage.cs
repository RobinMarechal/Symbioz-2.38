using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolPartyRefreshMessage : Message {
        public const ushort Id = 6583;

        public override ushort MessageId {
            get { return Id; }
        }

        public PartyIdol partyIdol;


        public IdolPartyRefreshMessage() { }

        public IdolPartyRefreshMessage(PartyIdol partyIdol) {
            this.partyIdol = partyIdol;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.partyIdol.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.partyIdol = new PartyIdol();
            this.partyIdol.Deserialize(reader);
        }
    }
}