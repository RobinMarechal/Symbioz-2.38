using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountDataMessage : Message {
        public const ushort Id = 5973;

        public override ushort MessageId {
            get { return Id; }
        }

        public MountClientData mountData;


        public MountDataMessage() { }

        public MountDataMessage(MountClientData mountData) {
            this.mountData = mountData;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.mountData.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mountData = new MountClientData();
            this.mountData.Deserialize(reader);
        }
    }
}