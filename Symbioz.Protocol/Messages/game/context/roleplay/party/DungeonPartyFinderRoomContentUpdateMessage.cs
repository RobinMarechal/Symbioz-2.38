using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DungeonPartyFinderRoomContentUpdateMessage : Message {
        public const ushort Id = 6250;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public DungeonPartyFinderPlayer[] addedPlayers;
        public ulong[] removedPlayersIds;


        public DungeonPartyFinderRoomContentUpdateMessage() { }

        public DungeonPartyFinderRoomContentUpdateMessage(ushort dungeonId, DungeonPartyFinderPlayer[] addedPlayers, ulong[] removedPlayersIds) {
            this.dungeonId = dungeonId;
            this.addedPlayers = addedPlayers;
            this.removedPlayersIds = removedPlayersIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteUShort((ushort) this.addedPlayers.Length);
            foreach (var entry in this.addedPlayers) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.removedPlayersIds.Length);
            foreach (var entry in this.removedPlayersIds) {
                writer.WriteVarUhLong(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            this.addedPlayers = new DungeonPartyFinderPlayer[limit];
            for (int i = 0; i < limit; i++) {
                this.addedPlayers[i] = new DungeonPartyFinderPlayer();
                this.addedPlayers[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.removedPlayersIds = new ulong[limit];
            for (int i = 0; i < limit; i++) {
                this.removedPlayersIds[i] = reader.ReadVarUhLong();
            }
        }
    }
}