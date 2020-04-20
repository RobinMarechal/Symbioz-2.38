using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicWhoIsMessage : Message {
        public const ushort Id = 180;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool self;
        public bool verbose;
        public sbyte position;
        public string accountNickname;
        public int accountId;
        public string playerName;
        public ulong playerId;
        public short areaId;
        public short serverId;
        public AbstractSocialGroupInfos[] socialGroups;
        public sbyte playerState;


        public BasicWhoIsMessage() { }

        public BasicWhoIsMessage(bool self,
                                 bool verbose,
                                 sbyte position,
                                 string accountNickname,
                                 int accountId,
                                 string playerName,
                                 ulong playerId,
                                 short areaId,
                                 short serverId,
                                 AbstractSocialGroupInfos[] socialGroups,
                                 sbyte playerState) {
            this.self = self;
            this.verbose = verbose;
            this.position = position;
            this.accountNickname = accountNickname;
            this.accountId = accountId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.areaId = areaId;
            this.serverId = serverId;
            this.socialGroups = socialGroups;
            this.playerState = playerState;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.self);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.verbose);
            writer.WriteByte(flag1);
            writer.WriteSByte(this.position);
            writer.WriteUTF(this.accountNickname);
            writer.WriteInt(this.accountId);
            writer.WriteUTF(this.playerName);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteShort(this.areaId);
            writer.WriteShort(this.serverId);
            writer.WriteUShort((ushort) this.socialGroups.Length);
            foreach (var entry in this.socialGroups) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteSByte(this.playerState);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.self = BooleanByteWrapper.GetFlag(flag1, 0);
            this.verbose = BooleanByteWrapper.GetFlag(flag1, 1);
            this.position = reader.ReadSByte();
            this.accountNickname = reader.ReadUTF();
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.playerName = reader.ReadUTF();
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.areaId = reader.ReadShort();
            this.serverId = reader.ReadShort();
            var limit = reader.ReadUShort();
            this.socialGroups = new AbstractSocialGroupInfos[limit];
            for (int i = 0; i < limit; i++) {
                this.socialGroups[i] = ProtocolTypeManager.GetInstance<AbstractSocialGroupInfos>(reader.ReadShort());
                this.socialGroups[i].Deserialize(reader);
            }

            this.playerState = reader.ReadSByte();

            if (this.playerState < 0)
                throw new Exception("Forbidden value on playerState = " + this.playerState + ", it doesn't respect the following condition : playerState < 0");
        }
    }
}