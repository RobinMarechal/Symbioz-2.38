using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChallengeInfoMessage : Message {
        public const ushort Id = 6022;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort challengeId;
        public double targetId;
        public uint xpBonus;
        public uint dropBonus;


        public ChallengeInfoMessage() { }

        public ChallengeInfoMessage(ushort challengeId, double targetId, uint xpBonus, uint dropBonus) {
            this.challengeId = challengeId;
            this.targetId = targetId;
            this.xpBonus = xpBonus;
            this.dropBonus = dropBonus;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.challengeId);
            writer.WriteDouble(this.targetId);
            writer.WriteVarUhInt(this.xpBonus);
            writer.WriteVarUhInt(this.dropBonus);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.challengeId = reader.ReadVarUhShort();

            if (this.challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + this.challengeId + ", it doesn't respect the following condition : challengeId < 0");
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.xpBonus = reader.ReadVarUhInt();

            if (this.xpBonus < 0)
                throw new Exception("Forbidden value on xpBonus = " + this.xpBonus + ", it doesn't respect the following condition : xpBonus < 0");
            this.dropBonus = reader.ReadVarUhInt();

            if (this.dropBonus < 0)
                throw new Exception("Forbidden value on dropBonus = " + this.dropBonus + ", it doesn't respect the following condition : dropBonus < 0");
        }
    }
}