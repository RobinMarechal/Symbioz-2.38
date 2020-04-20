// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TaxCollectorWaitingForHelpInformations : TaxCollectorComplementaryInformations {
        public const short Id = 447;

        public override short TypeId {
            get { return Id; }
        }

        public ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;


        public TaxCollectorWaitingForHelpInformations() { }

        public TaxCollectorWaitingForHelpInformations(ProtectedEntityWaitingForHelpInfo waitingForHelpInfo) {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.waitingForHelpInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
            this.waitingForHelpInfo.Deserialize(reader);
        }
    }
}