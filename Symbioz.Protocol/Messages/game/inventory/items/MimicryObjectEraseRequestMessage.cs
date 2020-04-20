using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MimicryObjectEraseRequestMessage : Message {
        public const ushort Id = 6457;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint hostUID;
        public byte hostPos;


        public MimicryObjectEraseRequestMessage() { }

        public MimicryObjectEraseRequestMessage(uint hostUID, byte hostPos) {
            this.hostUID = hostUID;
            this.hostPos = hostPos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.hostUID);
            writer.WriteByte(this.hostPos);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.hostUID = reader.ReadVarUhInt();

            if (this.hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + this.hostUID + ", it doesn't respect the following condition : hostUID < 0");
            this.hostPos = reader.ReadByte();

            if (this.hostPos < 0 || this.hostPos > 255)
                throw new Exception("Forbidden value on hostPos = " + this.hostPos + ", it doesn't respect the following condition : hostPos < 0 || hostPos > 255");
        }
    }
}