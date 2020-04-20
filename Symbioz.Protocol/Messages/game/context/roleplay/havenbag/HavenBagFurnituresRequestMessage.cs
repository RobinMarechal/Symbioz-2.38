using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HavenBagFurnituresRequestMessage : Message {
        public const ushort Id = 6637;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] cellIds;
        public int[] funitureIds;
        public sbyte[] orientations;


        public HavenBagFurnituresRequestMessage() { }

        public HavenBagFurnituresRequestMessage(ushort[] cellIds, int[] funitureIds, sbyte[] orientations) {
            this.cellIds = cellIds;
            this.funitureIds = funitureIds;
            this.orientations = orientations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.cellIds.Length);
            foreach (var entry in this.cellIds) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.funitureIds.Length);
            foreach (var entry in this.funitureIds) {
                writer.WriteInt(entry);
            }

            writer.WriteUShort((ushort) this.orientations.Length);
            foreach (var entry in this.orientations) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.cellIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.cellIds[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.funitureIds = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.funitureIds[i] = reader.ReadInt();
            }

            limit = reader.ReadUShort();
            this.orientations = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.orientations[i] = reader.ReadSByte();
            }
        }
    }
}