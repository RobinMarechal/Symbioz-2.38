using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChallengeResultMessage : Message {
        public const ushort Id = 6019;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort challengeId;
        public bool success;


        public ChallengeResultMessage() { }

        public ChallengeResultMessage(ushort challengeId, bool success) {
            this.challengeId = challengeId;
            this.success = success;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.challengeId);
            writer.WriteBoolean(this.success);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.challengeId = reader.ReadVarUhShort();

            if (this.challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + this.challengeId + ", it doesn't respect the following condition : challengeId < 0");
            this.success = reader.ReadBoolean();
        }
    }
}