using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LockableUseCodeMessage : Message {
        public const ushort Id = 5667;

        public override ushort MessageId {
            get { return Id; }
        }

        public string code;


        public LockableUseCodeMessage() { }

        public LockableUseCodeMessage(string code) {
            this.code = code;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.code);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.code = reader.ReadUTF();
        }
    }
}