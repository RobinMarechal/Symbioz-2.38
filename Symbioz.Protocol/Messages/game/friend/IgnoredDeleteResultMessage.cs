using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IgnoredDeleteResultMessage : Message {
        public const ushort Id = 5677;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool success;
        public bool session;
        public string name;


        public IgnoredDeleteResultMessage() { }

        public IgnoredDeleteResultMessage(bool success, bool session, string name) {
            this.success = success;
            this.session = session;
            this.name = name;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.success);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.session);
            writer.WriteByte(flag1);
            writer.WriteUTF(this.name);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.success = BooleanByteWrapper.GetFlag(flag1, 0);
            this.session = BooleanByteWrapper.GetFlag(flag1, 1);
            this.name = reader.ReadUTF();
        }
    }
}