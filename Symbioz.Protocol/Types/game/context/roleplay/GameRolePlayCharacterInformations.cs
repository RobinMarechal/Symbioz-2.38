// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayCharacterInformations : GameRolePlayHumanoidInformations {
        public const short Id = 36;

        public override short TypeId {
            get { return Id; }
        }

        public ActorAlignmentInformations alignmentInfos;


        public GameRolePlayCharacterInformations() { }

        public GameRolePlayCharacterInformations(double contextualId,
                                                 EntityLook look,
                                                 EntityDispositionInformations disposition,
                                                 string name,
                                                 HumanInformations humanoidInfo,
                                                 int accountId,
                                                 ActorAlignmentInformations alignmentInfos)
            : base(contextualId, look, disposition, name, humanoidInfo, accountId) {
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