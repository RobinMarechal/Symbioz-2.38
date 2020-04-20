using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextMoveElementMessage : Message {
        public const ushort Id = 253;

        public override ushort MessageId {
            get { return Id; }
        }

        public EntityMovementInformations movement;


        public GameContextMoveElementMessage() { }

        public GameContextMoveElementMessage(EntityMovementInformations movement) {
            this.movement = movement;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.movement.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.movement = new EntityMovementInformations();
            this.movement.Deserialize(reader);
        }
    }
}