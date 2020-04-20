using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SymbioticObjectAssociatedMessage : Message {
        public const ushort Id = 6527;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint hostUID;


        public SymbioticObjectAssociatedMessage() { }

        public SymbioticObjectAssociatedMessage(uint hostUID) {
            this.hostUID = hostUID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.hostUID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.hostUID = reader.ReadVarUhInt();

            if (this.hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + this.hostUID + ", it doesn't respect the following condition : hostUID < 0");
        }
    }
}