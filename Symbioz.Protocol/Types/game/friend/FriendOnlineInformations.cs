// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FriendOnlineInformations : FriendInformations {
        public const short Id = 92;

        public override short TypeId {
            get { return Id; }
        }

        public ulong playerId;
        public string playerName;
        public byte level;
        public sbyte alignmentSide;
        public sbyte breed;
        public bool sex;
        public BasicGuildInformations guildInfo;
        public ushort moodSmileyId;
        public PlayerStatus status;


        public FriendOnlineInformations() { }

        public FriendOnlineInformations(int accountId,
                                        string accountName,
                                        sbyte playerState,
                                        ushort lastConnection,
                                        int achievementPoints,
                                        ulong playerId,
                                        string playerName,
                                        byte level,
                                        sbyte alignmentSide,
                                        sbyte breed,
                                        bool sex,
                                        BasicGuildInformations guildInfo,
                                        ushort moodSmileyId,
                                        PlayerStatus status)
            : base(accountId, accountName, playerState, lastConnection, achievementPoints) {
            this.playerId = playerId;
            this.playerName = playerName;
            this.level = level;
            this.alignmentSide = alignmentSide;
            this.breed = breed;
            this.sex = sex;
            this.guildInfo = guildInfo;
            this.moodSmileyId = moodSmileyId;
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteUTF(this.playerName);
            writer.WriteByte(this.level);
            writer.WriteSByte(this.alignmentSide);
            writer.WriteSByte(this.breed);
            writer.WriteBoolean(this.sex);
            this.guildInfo.Serialize(writer);
            writer.WriteVarUhShort(this.moodSmileyId);
            writer.WriteShort(this.status.TypeId);
            this.status.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.playerName = reader.ReadUTF();
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 200)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 200");
            this.alignmentSide = reader.ReadSByte();
            this.breed = reader.ReadSByte();

            if (this.breed < (byte) Enums.PlayableBreedEnum.Feca || this.breed > (byte) Enums.PlayableBreedEnum.Huppermage)
                throw new Exception("Forbidden value on breed = "
                                    + this.breed
                                    + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Huppermage");
            this.sex = reader.ReadBoolean();
            this.guildInfo = new BasicGuildInformations();
            this.guildInfo.Deserialize(reader);
            this.moodSmileyId = reader.ReadVarUhShort();

            if (this.moodSmileyId < 0)
                throw new Exception("Forbidden value on moodSmileyId = " + this.moodSmileyId + ", it doesn't respect the following condition : moodSmileyId < 0");
            this.status = ProtocolTypeManager.GetInstance<PlayerStatus>(reader.ReadShort());
            this.status.Deserialize(reader);
        }
    }
}