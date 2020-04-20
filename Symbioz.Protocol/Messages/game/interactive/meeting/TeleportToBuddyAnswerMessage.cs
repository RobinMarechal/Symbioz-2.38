using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportToBuddyAnswerMessage : Message {
        public const ushort Id = 6293;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public ulong buddyId;
        public bool accept;


        public TeleportToBuddyAnswerMessage() { }

        public TeleportToBuddyAnswerMessage(ushort dungeonId, ulong buddyId, bool accept) {
            this.dungeonId = dungeonId;
            this.buddyId = buddyId;
            this.accept = accept;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteVarUhLong(this.buddyId);
            writer.WriteBoolean(this.accept);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            this.buddyId = reader.ReadVarUhLong();

            if (this.buddyId < 0 || this.buddyId > 9007199254740990)
                throw new Exception("Forbidden value on buddyId = " + this.buddyId + ", it doesn't respect the following condition : buddyId < 0 || buddyId > 9007199254740990");
            this.accept = reader.ReadBoolean();
        }
    }
}