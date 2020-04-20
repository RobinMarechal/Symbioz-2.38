using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectMovementMessage : Message {
        public const ushort Id = 3010;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint objectUID;
        public byte position;


        public ObjectMovementMessage() { }

        public ObjectMovementMessage(uint objectUID, byte position) {
            this.objectUID = objectUID;
            this.position = position;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.objectUID);
            writer.WriteByte(this.position);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.objectUID = reader.ReadVarUhInt();

            if (this.objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + this.objectUID + ", it doesn't respect the following condition : objectUID < 0");
            this.position = reader.ReadByte();

            if (this.position < 0 || this.position > 255)
                throw new Exception("Forbidden value on position = " + this.position + ", it doesn't respect the following condition : position < 0 || position > 255");
        }
    }
}