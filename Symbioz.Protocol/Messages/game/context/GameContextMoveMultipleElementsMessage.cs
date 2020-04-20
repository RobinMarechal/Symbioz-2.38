using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextMoveMultipleElementsMessage : Message {
        public const ushort Id = 254;

        public override ushort MessageId {
            get { return Id; }
        }

        public EntityMovementInformations[] movements;


        public GameContextMoveMultipleElementsMessage() { }

        public GameContextMoveMultipleElementsMessage(EntityMovementInformations[] movements) {
            this.movements = movements;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.movements.Length);
            foreach (var entry in this.movements) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.movements = new EntityMovementInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.movements[i] = new EntityMovementInformations();
                this.movements[i].Deserialize(reader);
            }
        }
    }
}