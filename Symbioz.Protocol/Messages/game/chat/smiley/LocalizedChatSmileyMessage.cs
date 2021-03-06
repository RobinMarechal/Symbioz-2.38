using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LocalizedChatSmileyMessage : ChatSmileyMessage {
        public const ushort Id = 6185;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort cellId;


        public LocalizedChatSmileyMessage() { }

        public LocalizedChatSmileyMessage(double entityId, ushort smileyId, int accountId, ushort cellId)
            : base(entityId, smileyId, accountId) {
            this.cellId = cellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.cellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.cellId = reader.ReadVarUhShort();

            if (this.cellId < 0 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
    }
}