using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyUpdateMessage : AbstractPartyEventMessage {
        public const ushort Id = 5575;

        public override ushort MessageId {
            get { return Id; }
        }

        public PartyMemberInformations memberInformations;


        public PartyUpdateMessage() { }

        public PartyUpdateMessage(uint partyId, PartyMemberInformations memberInformations)
            : base(partyId) {
            this.memberInformations = memberInformations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.memberInformations.TypeId);
            this.memberInformations.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.memberInformations = ProtocolTypeManager.GetInstance<PartyMemberInformations>(reader.ReadShort());
            this.memberInformations.Deserialize(reader);
        }
    }
}