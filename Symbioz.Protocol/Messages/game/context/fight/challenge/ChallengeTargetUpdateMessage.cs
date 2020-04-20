using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChallengeTargetUpdateMessage : Message {
        public const ushort Id = 6123;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort challengeId;
        public double targetId;


        public ChallengeTargetUpdateMessage() { }

        public ChallengeTargetUpdateMessage(ushort challengeId, double targetId) {
            this.challengeId = challengeId;
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.challengeId);
            writer.WriteDouble(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.challengeId = reader.ReadVarUhShort();

            if (this.challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + this.challengeId + ", it doesn't respect the following condition : challengeId < 0");
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
        }
    }
}