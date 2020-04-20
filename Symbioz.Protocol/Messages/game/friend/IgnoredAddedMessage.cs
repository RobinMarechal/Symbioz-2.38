using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IgnoredAddedMessage : Message {
        public const ushort Id = 5678;

        public override ushort MessageId {
            get { return Id; }
        }

        public IgnoredInformations ignoreAdded;
        public bool session;


        public IgnoredAddedMessage() { }

        public IgnoredAddedMessage(IgnoredInformations ignoreAdded, bool session) {
            this.ignoreAdded = ignoreAdded;
            this.session = session;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.ignoreAdded.TypeId);
            this.ignoreAdded.Serialize(writer);
            writer.WriteBoolean(this.session);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.ignoreAdded = ProtocolTypeManager.GetInstance<IgnoredInformations>(reader.ReadShort());
            this.ignoreAdded.Deserialize(reader);
            this.session = reader.ReadBoolean();
        }
    }
}