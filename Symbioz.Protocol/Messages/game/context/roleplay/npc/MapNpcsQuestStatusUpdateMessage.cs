using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapNpcsQuestStatusUpdateMessage : Message {
        public const ushort Id = 5642;

        public override ushort MessageId {
            get { return Id; }
        }

        public int mapId;
        public int[] npcsIdsWithQuest;
        public GameRolePlayNpcQuestFlag[] questFlags;
        public int[] npcsIdsWithoutQuest;


        public MapNpcsQuestStatusUpdateMessage() { }

        public MapNpcsQuestStatusUpdateMessage(int mapId, int[] npcsIdsWithQuest, GameRolePlayNpcQuestFlag[] questFlags, int[] npcsIdsWithoutQuest) {
            this.mapId = mapId;
            this.npcsIdsWithQuest = npcsIdsWithQuest;
            this.questFlags = questFlags;
            this.npcsIdsWithoutQuest = npcsIdsWithoutQuest;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.mapId);
            writer.WriteUShort((ushort) this.npcsIdsWithQuest.Length);
            foreach (var entry in this.npcsIdsWithQuest) {
                writer.WriteInt(entry);
            }

            writer.WriteUShort((ushort) this.questFlags.Length);
            foreach (var entry in this.questFlags) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.npcsIdsWithoutQuest.Length);
            foreach (var entry in this.npcsIdsWithoutQuest) {
                writer.WriteInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            this.npcsIdsWithQuest = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.npcsIdsWithQuest[i] = reader.ReadInt();
            }

            limit = reader.ReadUShort();
            this.questFlags = new GameRolePlayNpcQuestFlag[limit];
            for (int i = 0; i < limit; i++) {
                this.questFlags[i] = new GameRolePlayNpcQuestFlag();
                this.questFlags[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.npcsIdsWithoutQuest = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.npcsIdsWithoutQuest[i] = reader.ReadInt();
            }
        }
    }
}