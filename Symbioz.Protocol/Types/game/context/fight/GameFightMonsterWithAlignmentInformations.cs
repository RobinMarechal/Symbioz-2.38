// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightMonsterWithAlignmentInformations : GameFightMonsterInformations {
        public const short Id = 203;

        public override short TypeId {
            get { return Id; }
        }

        public ActorAlignmentInformations alignmentInfos;


        public GameFightMonsterWithAlignmentInformations() { }

        public GameFightMonsterWithAlignmentInformations(double contextualId,
                                                         EntityLook look,
                                                         EntityDispositionInformations disposition,
                                                         sbyte teamId,
                                                         sbyte wave,
                                                         bool alive,
                                                         GameFightMinimalStats stats,
                                                         ushort[] previousPositions,
                                                         ushort creatureGenericId,
                                                         sbyte creatureGrade,
                                                         ActorAlignmentInformations alignmentInfos)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions, creatureGenericId, creatureGrade) {
            this.alignmentInfos = alignmentInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.alignmentInfos.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.alignmentInfos = new ActorAlignmentInformations();
            this.alignmentInfos.Deserialize(reader);
        }
    }
}