using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyFollowStatusUpdateMessage : AbstractPartyMessage {
        public const ushort Id = 5581;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool success;
        public bool isFollowed;
        public ulong followedId;


        public PartyFollowStatusUpdateMessage() { }

        public PartyFollowStatusUpdateMessage(uint partyId, bool success, bool isFollowed, ulong followedId)
            : base(partyId) {
            this.success = success;
            this.isFollowed = isFollowed;
            this.followedId = followedId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.success);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.isFollowed);
            writer.WriteByte(flag1);
            writer.WriteVarUhLong(this.followedId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            this.success = BooleanByteWrapper.GetFlag(flag1, 0);
            this.isFollowed = BooleanByteWrapper.GetFlag(flag1, 1);
            this.followedId = reader.ReadVarUhLong();

            if (this.followedId < 0 || this.followedId > 9007199254740990)
                throw new Exception("Forbidden value on followedId = " + this.followedId + ", it doesn't respect the following condition : followedId < 0 || followedId > 9007199254740990");
        }
    }
}