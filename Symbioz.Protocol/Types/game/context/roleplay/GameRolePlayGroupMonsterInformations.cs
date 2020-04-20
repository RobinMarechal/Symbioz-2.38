// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations {
        public const short Id = 160;

        public override short TypeId {
            get { return Id; }
        }

        public bool keyRingBonus;
        public bool hasHardcoreDrop;
        public bool hasAVARewardToken;
        public GroupMonsterStaticInformations staticInfos;
        public double creationDate;
        public uint ageBonusRate;
        public sbyte lootShare;
        public sbyte alignmentSide;


        public GameRolePlayGroupMonsterInformations() { }

        public GameRolePlayGroupMonsterInformations(int contextualId,
                                                    EntityLook look,
                                                    EntityDispositionInformations disposition,
                                                    bool keyRingBonus,
                                                    bool hasHardcoreDrop,
                                                    bool hasAVARewardToken,
                                                    GroupMonsterStaticInformations staticInfos,
                                                    double creationDate,
                                                    uint ageBonus,
                                                    sbyte lootShare,
                                                    sbyte alignmentSide)
            : base(contextualId, look, disposition) {
            this.keyRingBonus = keyRingBonus;
            this.hasHardcoreDrop = hasHardcoreDrop;
            this.hasAVARewardToken = hasAVARewardToken;
            this.staticInfos = staticInfos;
            this.creationDate = creationDate;
            this.ageBonusRate = ageBonus;
            this.lootShare = lootShare;
            this.alignmentSide = alignmentSide;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.keyRingBonus);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.hasHardcoreDrop);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, this.hasAVARewardToken);
            writer.WriteByte(flag1);
            writer.WriteShort(this.staticInfos.TypeId);
            this.staticInfos.Serialize(writer);
            writer.WriteDouble(this.creationDate);
            writer.WriteUInt(this.ageBonusRate);
            writer.WriteByte((byte) this.lootShare);
            writer.WriteByte((byte) this.alignmentSide);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            this.keyRingBonus = BooleanByteWrapper.GetFlag(flag1, 0);
            this.hasHardcoreDrop = BooleanByteWrapper.GetFlag(flag1, 1);
            this.hasAVARewardToken = BooleanByteWrapper.GetFlag(flag1, 2);
            this.staticInfos = ProtocolTypeManager.GetInstance<GroupMonsterStaticInformations>(reader.ReadShort());
            this.staticInfos.Deserialize(reader);
            this.creationDate = reader.ReadDouble();
            this.ageBonusRate = reader.ReadUInt();
            this.lootShare = reader.ReadSByte();

            if ((this.lootShare < -1) || (this.lootShare > 8))
                throw new Exception("Forbidden value on lootShare = " + this.lootShare + ", it doesn't respect the following condition : (lootShare < -1) || (lootShare > 8)");
            this.alignmentSide = reader.ReadSByte();
        }
    }
}