using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InventoryPresetSaveCustomMessage : Message {
        public const ushort Id = 6329;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte presetId;
        public sbyte symbolId;
        public byte[] itemsPositions;
        public uint[] itemsUids;


        public InventoryPresetSaveCustomMessage() { }

        public InventoryPresetSaveCustomMessage(sbyte presetId, sbyte symbolId, byte[] itemsPositions, uint[] itemsUids) {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.itemsPositions = itemsPositions;
            this.itemsUids = itemsUids;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.presetId);
            writer.WriteSByte(this.symbolId);
            writer.WriteUShort((ushort) this.itemsPositions.Length);
            foreach (var entry in this.itemsPositions) {
                writer.WriteByte(entry);
            }

            writer.WriteUShort((ushort) this.itemsUids.Length);
            foreach (var entry in this.itemsUids) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.presetId = reader.ReadSByte();

            if (this.presetId < 0)
                throw new Exception("Forbidden value on presetId = " + this.presetId + ", it doesn't respect the following condition : presetId < 0");
            this.symbolId = reader.ReadSByte();

            if (this.symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + this.symbolId + ", it doesn't respect the following condition : symbolId < 0");
            var limit = reader.ReadUShort();
            this.itemsPositions = new byte[limit];
            for (int i = 0; i < limit; i++) {
                this.itemsPositions[i] = reader.ReadByte();
            }

            limit = reader.ReadUShort();
            this.itemsUids = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.itemsUids[i] = reader.ReadVarUhInt();
            }
        }
    }
}