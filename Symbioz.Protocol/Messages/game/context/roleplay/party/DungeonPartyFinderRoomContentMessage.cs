using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DungeonPartyFinderRoomContentMessage : Message {
        public const ushort Id = 6247;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public DungeonPartyFinderPlayer[] players;


        public DungeonPartyFinderRoomContentMessage() { }

        public DungeonPartyFinderRoomContentMessage(ushort dungeonId, DungeonPartyFinderPlayer[] players) {
            this.dungeonId = dungeonId;
            this.players = players;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteUShort((ushort) this.players.Length);
            foreach (var entry in this.players) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            this.players = new DungeonPartyFinderPlayer[limit];
            for (int i = 0; i < limit; i++) {
                this.players[i] = new DungeonPartyFinderPlayer();
                this.players[i].Deserialize(reader);
            }
        }
    }
}