// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayPrismInformations : GameRolePlayActorInformations {
        public const short Id = 161;

        public override short TypeId {
            get { return Id; }
        }

        public PrismInformation prism;

        public GameRolePlayPrismInformations() { }

        public GameRolePlayPrismInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, PrismInformation prism)
            : base(contextualId, look, disposition) {
            this.prism = prism;
        }

        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.prism.TypeId);
            this.prism.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.prism = ProtocolTypeManager.GetInstance<PrismInformation>(reader.ReadShort());
            this.prism.Deserialize(reader);
        }
    }
}