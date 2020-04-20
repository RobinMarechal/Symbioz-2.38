using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyCancelInvitationNotificationMessage : AbstractPartyEventMessage {
        public const ushort Id = 6251;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong cancelerId;
        public ulong guestId;


        public PartyCancelInvitationNotificationMessage() { }

        public PartyCancelInvitationNotificationMessage(uint partyId, ulong cancelerId, ulong guestId)
            : base(partyId) {
            this.cancelerId = cancelerId;
            this.guestId = guestId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.cancelerId);
            writer.WriteVarUhLong(this.guestId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.cancelerId = reader.ReadVarUhLong();

            if (this.cancelerId < 0 || this.cancelerId > 9007199254740990)
                throw new Exception("Forbidden value on cancelerId = " + this.cancelerId + ", it doesn't respect the following condition : cancelerId < 0 || cancelerId > 9007199254740990");
            this.guestId = reader.ReadVarUhLong();

            if (this.guestId < 0 || this.guestId > 9007199254740990)
                throw new Exception("Forbidden value on guestId = " + this.guestId + ", it doesn't respect the following condition : guestId < 0 || guestId > 9007199254740990");
        }
    }
}