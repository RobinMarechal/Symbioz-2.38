using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountRenamedMessage : Message {
        public const ushort Id = 5983;

        public override ushort MessageId {
            get { return Id; }
        }

        public int mountId;
        public string name;


        public MountRenamedMessage() { }

        public MountRenamedMessage(int mountId, string name) {
            this.mountId = mountId;
            this.name = name;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarInt(this.mountId);
            writer.WriteUTF(this.name);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mountId = reader.ReadVarInt();
            this.name = reader.ReadUTF();
        }
    }
}