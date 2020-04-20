using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AbstractPartyMessage : Message {
        public const ushort Id = 6274;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint partyId;


        public AbstractPartyMessage() { }

        public AbstractPartyMessage(uint partyId) {
            this.partyId = partyId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.partyId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.partyId = reader.ReadVarUhInt();

            if (this.partyId < 0)
                throw new Exception("Forbidden value on partyId = " + this.partyId + ", it doesn't respect the following condition : partyId < 0");
        }
    }
}