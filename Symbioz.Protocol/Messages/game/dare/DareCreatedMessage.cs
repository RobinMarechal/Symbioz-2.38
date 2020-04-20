using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareCreatedMessage : Message {
        public const ushort Id = 6668;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareInformations dareInfos;
        public bool needNotifications;


        public DareCreatedMessage() { }

        public DareCreatedMessage(DareInformations dareInfos, bool needNotifications) {
            this.dareInfos = dareInfos;
            this.needNotifications = needNotifications;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.dareInfos.Serialize(writer);
            writer.WriteBoolean(this.needNotifications);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dareInfos = new DareInformations();
            this.dareInfos.Deserialize(reader);
            this.needNotifications = reader.ReadBoolean();
        }
    }
}