using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChallengeTargetsListRequestMessage : Message {
        public const ushort Id = 5614;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort challengeId;


        public ChallengeTargetsListRequestMessage() { }

        public ChallengeTargetsListRequestMessage(ushort challengeId) {
            this.challengeId = challengeId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.challengeId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.challengeId = reader.ReadVarUhShort();

            if (this.challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + this.challengeId + ", it doesn't respect the following condition : challengeId < 0");
        }
    }
}