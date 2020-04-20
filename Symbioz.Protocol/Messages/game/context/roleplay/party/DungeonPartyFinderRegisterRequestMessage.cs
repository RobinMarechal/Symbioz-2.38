using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DungeonPartyFinderRegisterRequestMessage : Message {
        public const ushort Id = 6249;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] dungeonIds;


        public DungeonPartyFinderRegisterRequestMessage() { }

        public DungeonPartyFinderRegisterRequestMessage(ushort[] dungeonIds) {
            this.dungeonIds = dungeonIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.dungeonIds.Length);
            foreach (var entry in this.dungeonIds) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.dungeonIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.dungeonIds[i] = reader.ReadVarUhShort();
            }
        }
    }
}