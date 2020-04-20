using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryPresetItemUpdateRequestMessage : Message {
        public const ushort Id = 6210;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public byte position;
        public uint objUid;


        public InventoryPresetItemUpdateRequestMessage() { }

        public InventoryPresetItemUpdateRequestMessage(sbyte presetId, byte position, uint objUid) {
            this.presetId = presetId;
            this.position = position;
            this.objUid = objUid;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteByte(this.position);
            writer.WriteVarUhInt(this.objUid);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.position = reader.ReadByte();

            if (this.position < 0 || this.position > 255)
                throw new Exception("Forbidden value on position = " + this.position + ", it doesn't respect the following condition : position < 0 || position > 255");
            this.objUid = reader.ReadVarUhInt();

            if (this.objUid < 0)
                throw new Exception("Forbidden value on objUid = " + this.objUid + ", it doesn't respect the following condition : objUid < 0");
        }
    }
}