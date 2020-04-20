using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyMemberInFightMessage : AbstractPartyMessage {
        public const ushort Id = 6342;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte reason;
        public ulong memberId;
        public int memberAccountId;
        public string memberName;
        public int fightId;
        public MapCoordinatesExtended fightMap;
        public short timeBeforeFightStart;


        public PartyMemberInFightMessage() { }

        public PartyMemberInFightMessage(uint partyId, sbyte reason, ulong memberId, int memberAccountId, string memberName, int fightId, MapCoordinatesExtended fightMap, short timeBeforeFightStart)
            : base(partyId) {
            this.reason = reason;
            this.memberId = memberId;
            this.memberAccountId = memberAccountId;
            this.memberName = memberName;
            this.fightId = fightId;
            this.fightMap = fightMap;
            this.timeBeforeFightStart = timeBeforeFightStart;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.reason);
            writer.WriteVarUhLong(this.memberId);
            writer.WriteInt(this.memberAccountId);
            writer.WriteUTF(this.memberName);
            writer.WriteInt(this.fightId);
            this.fightMap.Serialize(writer);
            writer.WriteVarShort(this.timeBeforeFightStart);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.reason = reader.ReadSByte();

            if (this.reason < 0)
                throw new Exception("Forbidden value on reason = " + this.reason + ", it doesn't respect the following condition : reason < 0");
            this.memberId = reader.ReadVarUhLong();

            if (this.memberId < 0 || this.memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + this.memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            this.memberAccountId = reader.ReadInt();

            if (this.memberAccountId < 0)
                throw new Exception("Forbidden value on memberAccountId = " + this.memberAccountId + ", it doesn't respect the following condition : memberAccountId < 0");
            this.memberName = reader.ReadUTF();
            this.fightId = reader.ReadInt();
            this.fightMap = new MapCoordinatesExtended();
            this.fightMap.Deserialize(reader);
            this.timeBeforeFightStart = reader.ReadVarShort();
        }
    }
}