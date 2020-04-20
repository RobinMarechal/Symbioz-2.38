using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectSetPositionMessage : Message {
        public const ushort Id = 3021;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;
        public byte position;
        public uint quantity;


        public ObjectSetPositionMessage() { }

        public ObjectSetPositionMessage(uint objectUID, byte position, uint quantity) {
            this.objectUID = objectUID;
            this.position = position;
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
            writer.WriteByte(this.position);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
            this.position = reader.ReadByte();

            if (this.position < 0 || this.position > 255)
                throw new Exception("Forbidden value on position = " + this.position + ", it doesn't respect the following condition : position < 0 || position > 255");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}