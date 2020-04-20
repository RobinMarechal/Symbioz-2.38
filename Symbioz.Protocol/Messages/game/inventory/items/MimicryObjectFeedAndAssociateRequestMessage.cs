using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MimicryObjectFeedAndAssociateRequestMessage : SymbioticObjectAssociateRequestMessage {
        public const ushort Id = 6460;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint foodUID;
        public byte foodPos;
        public bool preview;


        public MimicryObjectFeedAndAssociateRequestMessage() { }

        public MimicryObjectFeedAndAssociateRequestMessage(uint symbioteUID, byte symbiotePos, uint hostUID, byte hostPos, uint foodUID, byte foodPos, bool preview)
            : base(symbioteUID, symbiotePos, hostUID, hostPos) {
            this.foodUID = foodUID;
            this.foodPos = foodPos;
            this.preview = preview;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.foodUID);
            writer.WriteByte(this.foodPos);
            writer.WriteBoolean(this.preview);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.foodUID = reader.ReadVarUhInt();

            if (this.foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + this.foodUID + ", it doesn't respect the following condition : foodUID < 0");
            this.foodPos = reader.ReadByte();

            if (this.foodPos < 0 || this.foodPos > 255)
                throw new Exception("Forbidden value on foodPos = " + this.foodPos + ", it doesn't respect the following condition : foodPos < 0 || foodPos > 255");
            this.preview = reader.ReadBoolean();
        }
    }
}