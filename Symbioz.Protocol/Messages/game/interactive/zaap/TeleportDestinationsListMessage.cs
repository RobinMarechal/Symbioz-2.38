using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportDestinationsListMessage : Message {
        public const ushort Id = 5960;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte teleporterType;
        public int[] mapIds;
        public ushort[] subAreaIds;
        public ushort[] costs;
        public sbyte[] destTeleporterType;


        public TeleportDestinationsListMessage() { }

        public TeleportDestinationsListMessage(sbyte teleporterType, int[] mapIds, ushort[] subAreaIds, ushort[] costs, sbyte[] destTeleporterType) {
            this.teleporterType = teleporterType;
            this.mapIds = mapIds;
            this.subAreaIds = subAreaIds;
            this.costs = costs;
            this.destTeleporterType = destTeleporterType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.teleporterType);
            writer.WriteUShort((ushort) this.mapIds.Length);
            foreach (var entry in this.mapIds) {
                writer.WriteInt(entry);
            }

            writer.WriteUShort((ushort) this.subAreaIds.Length);
            foreach (var entry in this.subAreaIds) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.costs.Length);
            foreach (var entry in this.costs) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.destTeleporterType.Length);
            foreach (var entry in this.destTeleporterType) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.teleporterType = reader.ReadSByte();

            if (this.teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + this.teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            this.mapIds = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.mapIds[i] = reader.ReadInt();
            }

            limit = reader.ReadUShort();
            this.subAreaIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.subAreaIds[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.costs = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.costs[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.destTeleporterType = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.destTeleporterType[i] = reader.ReadSByte();
            }
        }
    }
}