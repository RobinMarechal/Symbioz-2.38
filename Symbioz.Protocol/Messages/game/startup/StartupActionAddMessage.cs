using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StartupActionAddMessage : Message {
        public const ushort Id = 6538;

        public override ushort MessageId {
            get { return Id; }
        }

        public StartupActionAddObject newAction;


        public StartupActionAddMessage() { }

        public StartupActionAddMessage(StartupActionAddObject newAction) {
            this.newAction = newAction;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.newAction.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.newAction = new StartupActionAddObject();
            this.newAction.Deserialize(reader);
        }
    }
}