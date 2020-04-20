using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportBuddiesRequestedMessage : Message {
        public const ushort Id = 6302;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public ulong inviterId;
        public ulong[] invalidBuddiesIds;


        public TeleportBuddiesRequestedMessage() { }

        public TeleportBuddiesRequestedMessage(ushort dungeonId, ulong inviterId, ulong[] invalidBuddiesIds) {
            this.dungeonId = dungeonId;
            this.inviterId = inviterId;
            this.invalidBuddiesIds = invalidBuddiesIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteVarUhLong(this.inviterId);
            writer.WriteUShort((ushort) this.invalidBuddiesIds.Length);
            foreach (var entry in this.invalidBuddiesIds) {
                writer.WriteVarUhLong(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            this.inviterId = reader.ReadVarUhLong();

            if (this.inviterId < 0 || this.inviterId > 9007199254740990)
                throw new Exception("Forbidden value on inviterId = " + this.inviterId + ", it doesn't respect the following condition : inviterId < 0 || inviterId > 9007199254740990");
            var limit = reader.ReadUShort();
            this.invalidBuddiesIds = new ulong[limit];
            for (int i = 0; i < limit; i++) {
                this.invalidBuddiesIds[i] = reader.ReadVarUhLong();
            }
        }
    }
}