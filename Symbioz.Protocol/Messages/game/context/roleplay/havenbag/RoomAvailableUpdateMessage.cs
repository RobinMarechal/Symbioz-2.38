using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class RoomAvailableUpdateMessage : Message {
        public const ushort Id = 6630;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte nbRoom;


        public RoomAvailableUpdateMessage() { }

        public RoomAvailableUpdateMessage(byte nbRoom) {
            this.nbRoom = nbRoom;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.nbRoom);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.nbRoom = reader.ReadByte();

            if (this.nbRoom < 0 || this.nbRoom > 255)
                throw new Exception("Forbidden value on nbRoom = " + this.nbRoom + ", it doesn't respect the following condition : nbRoom < 0 || nbRoom > 255");
        }
    }
}