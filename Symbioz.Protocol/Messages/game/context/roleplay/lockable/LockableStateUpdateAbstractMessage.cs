using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LockableStateUpdateAbstractMessage : Message {
        public const ushort Id = 5671;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool locked;


        public LockableStateUpdateAbstractMessage() { }

        public LockableStateUpdateAbstractMessage(bool locked) {
            this.locked = locked;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.locked);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.locked = reader.ReadBoolean();
        }
    }
}