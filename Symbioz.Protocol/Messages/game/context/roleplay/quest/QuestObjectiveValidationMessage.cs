using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class QuestObjectiveValidationMessage : Message {
        public const ushort Id = 6085;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort questId;
        public ushort objectiveId;


        public QuestObjectiveValidationMessage() { }

        public QuestObjectiveValidationMessage(ushort questId, ushort objectiveId) {
            this.questId = questId;
            this.objectiveId = objectiveId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.questId);
            writer.WriteVarUhShort(this.objectiveId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questId = reader.ReadVarUhShort();

            if (this.questId < 0)
                throw new Exception("Forbidden value on questId = " + this.questId + ", it doesn't respect the following condition : questId < 0");
            this.objectiveId = reader.ReadVarUhShort();

            if (this.objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + this.objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
        }
    }
}