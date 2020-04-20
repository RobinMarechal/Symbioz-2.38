using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeObjectModifiedInBagMessage : ExchangeObjectMessage {
        public const ushort Id = 6008;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem @object;


        public ExchangeObjectModifiedInBagMessage() { }

        public ExchangeObjectModifiedInBagMessage(bool remote, ObjectItem @object)
            : base(remote) {
            this.@object = @object;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.@object.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.@object = new ObjectItem();
            this.@object.Deserialize(reader);
        }
    }
}