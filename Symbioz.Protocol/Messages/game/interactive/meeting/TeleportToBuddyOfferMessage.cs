using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportToBuddyOfferMessage : Message {
        public const ushort Id = 6287;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public ulong buddyId;
        public uint timeLeft;


        public TeleportToBuddyOfferMessage() { }

        public TeleportToBuddyOfferMessage(ushort dungeonId, ulong buddyId, uint timeLeft) {
            this.dungeonId = dungeonId;
            this.buddyId = buddyId;
            this.timeLeft = timeLeft;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteVarUhLong(this.buddyId);
            writer.WriteVarUhInt(this.timeLeft);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            this.buddyId = reader.ReadVarUhLong();

            if (this.buddyId < 0 || this.buddyId > 9007199254740990)
                throw new Exception("Forbidden value on buddyId = " + this.buddyId + ", it doesn't respect the following condition : buddyId < 0 || buddyId > 9007199254740990");
            this.timeLeft = reader.ReadVarUhInt();

            if (this.timeLeft < 0)
                throw new Exception("Forbidden value on timeLeft = " + this.timeLeft + ", it doesn't respect the following condition : timeLeft < 0");
        }
    }
}