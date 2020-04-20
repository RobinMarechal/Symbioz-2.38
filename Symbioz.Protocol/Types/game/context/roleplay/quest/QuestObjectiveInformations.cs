// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class QuestObjectiveInformations {
        public const short Id = 385;

        public virtual short TypeId {
            get { return Id; }
        }

        public ushort objectiveId;
        public bool objectiveStatus;
        public string[] dialogParams;


        public QuestObjectiveInformations() { }

        public QuestObjectiveInformations(ushort objectiveId, bool objectiveStatus, string[] dialogParams) {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
            this.dialogParams = dialogParams;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.objectiveId);
            writer.WriteBoolean(this.objectiveStatus);
            writer.WriteUShort((ushort) this.dialogParams.Length);
            foreach (var entry in this.dialogParams) {
                writer.WriteUTF(entry);
            }
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.objectiveId = reader.ReadVarUhShort();

            if (this.objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + this.objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            this.objectiveStatus = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.dialogParams = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.dialogParams[i] = reader.ReadUTF();
            }
        }
    }
}