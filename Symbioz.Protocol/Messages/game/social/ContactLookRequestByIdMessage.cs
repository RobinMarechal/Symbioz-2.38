using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ContactLookRequestByIdMessage : ContactLookRequestMessage {
        public const ushort Id = 5935;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong playerId;


        public ContactLookRequestByIdMessage() { }

        public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, ulong playerId)
            : base(requestId, contactType) {
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