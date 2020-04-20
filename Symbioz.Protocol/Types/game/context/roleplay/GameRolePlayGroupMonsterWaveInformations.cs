// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayGroupMonsterWaveInformations : GameRolePlayGroupMonsterInformations {
        public const short Id = 464;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte nbWaves;
        public IEnumerable<GroupMonsterStaticInformations> alternatives;


        public GameRolePlayGroupMonsterWaveInformations() { }

        public GameRolePlayGroupMonsterWaveInformations(int contextualId,
                                                        EntityLook look,
                                                        EntityDispositionInformations disposition,
                                                        bool keyRingBonus,
                                                        bool hasHardcoreDrop,
                                                        bool hasAVARewardToken,
                                                        GroupMonsterStaticInformations staticInfos,
                                                        double creationDate,
                                                        uint ageBonus,
                                                        sbyte lootShare,
                                                        sbyte alignmentSide,
                                                        sbyte nbWaves,
                                                        IEnumerable<GroupMonsterStaticInformations> alternatives)
            : base(contextualId, look, disposition, keyRingBonus, hasHardcoreDrop, hasAVARewardToken, staticInfos, creationDate, ageBonus, lootShare, alignmentSide) {
            this.nbWaves = nbWaves;
            this.alternatives = alternatives;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.nbWaves);
            writer.WriteUShort((ushort) this.alternatives.Count());
            foreach (var entry in this.alternatives) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.nbWaves = reader.ReadSByte();

            if (this.nbWaves < 0)
                throw new Exception("Forbidden value on nbWaves = " + this.nbWaves + ", it doesn't respect the following condition : nbWaves < 0");
            var limit = reader.ReadUShort();
            this.alternatives = new GroupMonsterStaticInformations[limit];
            for (int i = 0; i < limit; i++) {
                (this.alternatives as GroupMonsterStaticInformations[])[i] = ProtocolTypeManager.GetInstance<GroupMonsterStaticInformations>(reader.ReadShort());
                (this.alternatives as GroupMonsterStaticInformations[])[i].Deserialize(reader);
            }
        }
    }
}