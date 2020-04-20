using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInformationsGeneralMessage : Message {
        public const ushort Id = 5557;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool enabled;
        public bool abandonnedPaddock;
        public byte level;
        public ulong expLevelFloor;
        public ulong experience;
        public ulong expNextLevelFloor;
        public int creationDate;
        public ushort nbTotalMembers;
        public ushort nbConnectedMembers;


        public GuildInformationsGeneralMessage() { }

        public GuildInformationsGeneralMessage(bool enabled,
                                               bool abandonnedPaddock,
                                               byte level,
                                               ulong expLevelFloor,
                                               ulong experience,
                                               ulong expNextLevelFloor,
                                               int creationDate,
                                               ushort nbTotalMembers,
                                               ushort nbConnectedMembers) {
            this.enabled = enabled;
            this.abandonnedPaddock = abandonnedPaddock;
            this.level = level;
            this.expLevelFloor = expLevelFloor;
            this.experience = experience;
            this.expNextLevelFloor = expNextLevelFloor;
            this.creationDate = creationDate;
            this.nbTotalMembers = nbTotalMembers;
            this.nbConnectedMembers = nbConnectedMembers;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.enabled);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.abandonnedPaddock);
            writer.WriteByte(flag1);
            writer.WriteByte(this.level);
            writer.WriteVarUhLong(this.expLevelFloor);
            writer.WriteVarUhLong(this.experience);
            writer.WriteVarUhLong(this.expNextLevelFloor);
            writer.WriteInt(this.creationDate);
            writer.WriteVarUhShort(this.nbTotalMembers);
            writer.WriteVarUhShort(this.nbConnectedMembers);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.enabled = BooleanByteWrapper.GetFlag(flag1, 0);
            this.abandonnedPaddock = BooleanByteWrapper.GetFlag(flag1, 1);
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 255)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 255");
            this.expLevelFloor = reader.ReadVarUhLong();

            if (this.expLevelFloor < 0 || this.expLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on expLevelFloor = " + this.expLevelFloor + ", it doesn't respect the following condition : expLevelFloor < 0 || expLevelFloor > 9007199254740990");
            this.experience = reader.ReadVarUhLong();

            if (this.experience < 0 || this.experience > 9007199254740990)
                throw new Exception("Forbidden value on experience = " + this.experience + ", it doesn't respect the following condition : experience < 0 || experience > 9007199254740990");
            this.expNextLevelFloor = reader.ReadVarUhLong();

            if (this.expNextLevelFloor < 0 || this.expNextLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on expNextLevelFloor = " + this.expNextLevelFloor + ", it doesn't respect the following condition : expNextLevelFloor < 0 || expNextLevelFloor > 9007199254740990");
            this.creationDate = reader.ReadInt();

            if (this.creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + this.creationDate + ", it doesn't respect the following condition : creationDate < 0");
            this.nbTotalMembers = reader.ReadVarUhShort();

            if (this.nbTotalMembers < 0)
                throw new Exception("Forbidden value on nbTotalMembers = " + this.nbTotalMembers + ", it doesn't respect the following condition : nbTotalMembers < 0");
            this.nbConnectedMembers = reader.ReadVarUhShort();

            if (this.nbConnectedMembers < 0)
                throw new Exception("Forbidden value on nbConnectedMembers = " + this.nbConnectedMembers + ", it doesn't respect the following condition : nbConnectedMembers < 0");
        }
    }
}