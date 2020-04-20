using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NpcDialogQuestionMessage : Message {
        public const ushort Id = 5617;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort messageId;
        public string[] dialogParams;
        public ushort[] visibleReplies;


        public NpcDialogQuestionMessage() { }

        public NpcDialogQuestionMessage(ushort messageId, string[] dialogParams, ushort[] visibleReplies) {
            this.messageId = messageId;
            this.dialogParams = dialogParams;
            this.visibleReplies = visibleReplies;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.messageId);
            writer.WriteUShort((ushort) this.dialogParams.Length);
            foreach (var entry in this.dialogParams) {
                writer.WriteUTF(entry);
            }

            writer.WriteUShort((ushort) this.visibleReplies.Length);
            foreach (var entry in this.visibleReplies) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.messageId = reader.ReadVarUhShort();

            if (this.messageId < 0)
                throw new Exception("Forbidden value on messageId = " + this.messageId + ", it doesn't respect the following condition : messageId < 0");
            var limit = reader.ReadUShort();
            this.dialogParams = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.dialogParams[i] = reader.ReadUTF();
            }

            limit = reader.ReadUShort();
            this.visibleReplies = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.visibleReplies[i] = reader.ReadVarUhShort();
            }
        }
    }
}