using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IgnoredAddRequestMessage : Message {
        public const ushort Id = 5673;

        public override ushort MessageId {
            get { return Id; }
        }

        public string name;
        public bool session;


        public IgnoredAddRequestMessage() { }

        public IgnoredAddRequestMessage(string name, bool session) {
            this.name = name;
            this.session = session;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.name);
            writer.WriteBoolean(this.session);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.name = reader.ReadUTF();
            this.session = reader.ReadBoolean();
        }
    }
}