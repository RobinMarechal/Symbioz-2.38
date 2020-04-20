using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameMapChangeOrientationRequestMessage : Message {
        public const ushort Id = 945;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte direction;


        public GameMapChangeOrientationRequestMessage() { }

        public GameMapChangeOrientationRequestMessage(sbyte direction) {
            this.direction = direction;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.direction);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.direction = reader.ReadSByte();

            if (this.direction < 0)
                throw new Exception("Forbidden value on direction = " + this.direction + ", it doesn't respect the following condition : direction < 0");
        }
    }
}