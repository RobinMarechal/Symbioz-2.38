using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyAbdicateThroneMessage : AbstractPartyMessage {
        public const ushort Id = 6080;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong playerId;


        public PartyAbdicateThroneMessage() { }

        public PartyAbdicateThroneMessage(uint partyId, ulong playerId)
            : base(partyId) {
            this.playerId = playerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.playerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
        }
    }
}