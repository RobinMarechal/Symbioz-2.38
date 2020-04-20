using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class QuestStepStartedMessage : Message {
        public const ushort Id = 6096;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort questId;
        public ushort stepId;


        public QuestStepStartedMessage() { }

        public QuestStepStartedMessage(ushort questId, ushort stepId) {
            this.questId = questId;
            this.stepId = stepId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.questId);
            writer.WriteVarUhShort(this.stepId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questId = reader.ReadVarUhShort();

            if (this.questId < 0)
                throw new Exception("Forbidden value on questId = " + this.questId + ", it doesn't respect the following condition : questId < 0");
            this.stepId = reader.ReadVarUhShort();

            if (this.stepId < 0)
                throw new Exception("Forbidden value on stepId = " + this.stepId + ", it doesn't respect the following condition : stepId < 0");
        }
    }
}