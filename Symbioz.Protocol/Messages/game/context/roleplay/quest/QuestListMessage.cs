using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class QuestListMessage : Message {
        public const ushort Id = 5626;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] finishedQuestsIds;
        public ushort[] finishedQuestsCounts;
        public QuestActiveInformations[] activeQuests;
        public ushort[] reinitDoneQuestsIds;


        public QuestListMessage() { }

        public QuestListMessage(ushort[] finishedQuestsIds, ushort[] finishedQuestsCounts, QuestActiveInformations[] activeQuests, ushort[] reinitDoneQuestsIds) {
            this.finishedQuestsIds = finishedQuestsIds;
            this.finishedQuestsCounts = finishedQuestsCounts;
            this.activeQuests = activeQuests;
            this.reinitDoneQuestsIds = reinitDoneQuestsIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.finishedQuestsIds.Length);
            foreach (var entry in this.finishedQuestsIds) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.finishedQuestsCounts.Length);
            foreach (var entry in this.finishedQuestsCounts) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.activeQuests.Length);
            foreach (var entry in this.activeQuests) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.reinitDoneQuestsIds.Length);
            foreach (var entry in this.reinitDoneQuestsIds) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.finishedQuestsIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.finishedQuestsIds[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.finishedQuestsCounts = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.finishedQuestsCounts[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.activeQuests = new QuestActiveInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.activeQuests[i] = ProtocolTypeManager.GetInstance<QuestActiveInformations>(reader.ReadShort());
                this.activeQuests[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.reinitDoneQuestsIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.reinitDoneQuestsIds[i] = reader.ReadVarUhShort();
            }
        }
    }
}