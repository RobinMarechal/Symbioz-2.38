using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeCraftResultWithObjectDescMessage : ExchangeCraftResultMessage {
        public const ushort Id = 5999;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItemNotInContainer objectInfo;


        public ExchangeCraftResultWithObjectDescMessage() { }

        public ExchangeCraftResultWithObjectDescMessage(sbyte craftResult, ObjectItemNotInContainer objectInfo)
            : base(craftResult) {
            this.objectInfo = objectInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.objectInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.objectInfo = new ObjectItemNotInContainer();
            this.objectInfo.Deserialize(reader);
        }
    }
}