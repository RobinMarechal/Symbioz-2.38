using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyMemberRemoveMessage : AbstractPartyEventMessage {
        public const ushort Id = 5579;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong leavingPlayerId;


        public PartyMemberRemoveMessage() { }

        public PartyMemberRemoveMessage(uint partyId, ulong leavingPlayerId)
            : base(partyId) {
            this.leavingPlayerId = leavingPlayerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.leavingPlayerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.leavingPlayerId = reader.ReadVarUhLong();

            if (this.leavingPlayerId < 0 || this.leavingPlayerId > 9007199254740990)
                throw new Exception("Forbidden value on leavingPlayerId = " + this.leavingPlayerId + ", it doesn't respect the following condition : leavingPlayerId < 0 || leavingPlayerId > 9007199254740990");
        }
    }
}