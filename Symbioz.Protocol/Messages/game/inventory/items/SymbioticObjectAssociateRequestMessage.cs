using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SymbioticObjectAssociateRequestMessage : Message {
        public const ushort Id = 6522;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint symbioteUID;
        public byte symbiotePos;
        public uint hostUID;
        public byte hostPos;


        public SymbioticObjectAssociateRequestMessage() { }

        public SymbioticObjectAssociateRequestMessage(uint symbioteUID, byte symbiotePos, uint hostUID, byte hostPos) {
            this.symbioteUID = symbioteUID;
            this.symbiotePos = symbiotePos;
            this.hostUID = hostUID;
            this.hostPos = hostPos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.symbioteUID);
            writer.WriteByte(this.symbiotePos);
            writer.WriteVarUhInt(this.hostUID);
            writer.WriteByte(this.hostPos);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.symbioteUID = reader.ReadVarUhInt();

            if (this.symbioteUID < 0)
                throw new Exception("Forbidden value on symbioteUID = " + this.symbioteUID + ", it doesn't respect the following condition : symbioteUID < 0");
            this.symbiotePos = reader.ReadByte();

            if (this.symbiotePos < 0 || this.symbiotePos > 255)
                throw new Exception("Forbidden value on symbiotePos = " + this.symbiotePos + ", it doesn't respect the following condition : symbiotePos < 0 || symbiotePos > 255");
            this.hostUID = reader.ReadVarUhInt();

            if (this.hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + this.hostUID + ", it doesn't respect the following condition : hostUID < 0");
            this.hostPos = reader.ReadByte();

            if (this.hostPos < 0 || this.hostPos > 255)
                throw new Exception("Forbidden value on hostPos = " + this.hostPos + ", it doesn't respect the following condition : hostPos < 0 || hostPos > 255");
        }
    }
}