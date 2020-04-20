using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyCannotJoinErrorMessage : AbstractPartyMessage {
        public const ushort Id = 5583;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte reason;


        public PartyCannotJoinErrorMessage() { }

        public PartyCannotJoinErrorMessage(uint partyId, sbyte reason)
            : base(partyId) {
            this.reason = reason;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.reason);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.reason = reader.ReadSByte();

            if (this.reason < 0)
                throw new Exception("Forbidden value on reason = " + this.reason + ", it doesn't respect the following condition : reason < 0");
        }
    }
}