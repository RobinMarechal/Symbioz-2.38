using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HavenBagPackListMessage : Message {
        public const ushort Id = 6620;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte[] packIds;


        public HavenBagPackListMessage() { }

        public HavenBagPackListMessage(sbyte[] packIds) {
            this.packIds = packIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.packIds.Length);
            foreach (var entry in this.packIds) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.packIds = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.packIds[i] = reader.ReadSByte();
            }
        }
    }
}