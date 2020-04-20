using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChangeHavenBagRoomRequestMessage : Message {
        public const ushort Id = 6638;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte roomId;


        public ChangeHavenBagRoomRequestMessage() { }

        public ChangeHavenBagRoomRequestMessage(sbyte roomId) {
            this.roomId = roomId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.roomId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.roomId = reader.ReadSByte();

            if (this.roomId < 0)
                throw new Exception("Forbidden value on roomId = " + this.roomId + ", it doesn't respect the following condition : roomId < 0");
        }
    }
}