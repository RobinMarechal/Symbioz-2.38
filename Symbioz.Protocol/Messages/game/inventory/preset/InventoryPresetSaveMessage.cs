using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryPresetSaveMessage : Message {
        public const ushort Id = 6165;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public sbyte symbolId;
        public bool saveEquipment;


        public InventoryPresetSaveMessage() { }

        public InventoryPresetSaveMessage(sbyte presetId, sbyte symbolId, bool saveEquipment) {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.saveEquipment = saveEquipment;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteSByte(this.symbolId);
            writer.WriteBoolean(this.saveEquipment);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.symbolId = reader.ReadSByte();

            if (this.symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + this.symbolId + ", it doesn't respect the following condition : symbolId < 0");
            this.saveEquipment = reader.ReadBoolean();
        }
    }
}