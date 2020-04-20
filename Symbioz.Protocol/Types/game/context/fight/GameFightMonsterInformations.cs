// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightMonsterInformations : GameFightAIInformations {
        public const short Id = 29;

        public override short TypeId {
            get { return Id; }
        }

        public ushort creatureGenericId;
        public sbyte creatureGrade;


        public GameFightMonsterInformations() { }

        public GameFightMonsterInformations(double contextualId,
                                            EntityLook look,
                                            EntityDispositionInformations disposition,
                                            sbyte teamId,
                                            sbyte wave,
                                            bool alive,
                                            GameFightMinimalStats stats,
                                            ushort[] previousPositions,
                                            ushort creatureGenericId,
                                            sbyte creatureGrade)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions) {
            this.creatureGenericId = creatureGenericId;
            this.creatureGrade = creatureGrade;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.creatureGenericId);
            writer.WriteSByte(this.creatureGrade);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.creatureGenericId = reader.ReadVarUhShort();

            if (this.creatureGenericId < 0)
                throw new Exception("Forbidden value on creatureGenericId = " + this.creatureGenericId + ", it doesn't respect the following condition : creatureGenericId < 0");
            this.creatureGrade = reader.ReadSByte();

            if (this.creatureGrade < 0)
                throw new Exception("Forbidden value on creatureGrade = " + this.creatureGrade + ", it doesn't respect the following condition : creatureGrade < 0");
        }
    }
}