using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameMapChangeOrientationMessage : Message {
        public const ushort Id = 946;

        public override ushort MessageId {
            get { return Id; }
        }

        public ActorOrientation orientation;


        public GameMapChangeOrientationMessage() { }

        public GameMapChangeOrientationMessage(ActorOrientation orientation) {
            this.orientation = orientation;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.orientation.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.orientation = new ActorOrientation();
            this.orientation.Deserialize(reader);
        }
    }
}