using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyInvitationDungeonDetailsMessage : PartyInvitationDetailsMessage {
        public const ushort Id = 6262;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public bool[] playersDungeonReady;


        public PartyInvitationDungeonDetailsMessage() { }

        public PartyInvitationDungeonDetailsMessage(uint partyId,
                                                    sbyte partyType,
                                                    string partyName,
                                                    ulong fromId,
                                                    string fromName,
                                                    ulong leaderId,
                                                    PartyInvitationMemberInformations[] members,
                                                    PartyGuestInformations[] guests,
                                                    ushort dungeonId,
                                                    bool[] playersDungeonReady)
            : base(partyId, partyType, partyName, fromId, fromName, leaderId, members, guests) {
            this.dungeonId = dungeonId;
            this.playersDungeonReady = playersDungeonReady;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteUShort((ushort) this.playersDungeonReady.Length);
            foreach (var entry in this.playersDungeonReady) {
                writer.WriteBoolean(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            this.playersDungeonReady = new bool[limit];
            for (int i = 0; i < limit; i++) {
                this.playersDungeonReady[i] = reader.ReadBoolean();
            }
        }
    }
}