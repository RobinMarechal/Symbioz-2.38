using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyInvitationDetailsMessage : AbstractPartyMessage {
        public const ushort Id = 6263;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte partyType;
        public string partyName;
        public ulong fromId;
        public string fromName;
        public ulong leaderId;
        public PartyInvitationMemberInformations[] members;
        public PartyGuestInformations[] guests;


        public PartyInvitationDetailsMessage() { }

        public PartyInvitationDetailsMessage(uint partyId,
                                             sbyte partyType,
                                             string partyName,
                                             ulong fromId,
                                             string fromName,
                                             ulong leaderId,
                                             PartyInvitationMemberInformations[] members,
                                             PartyGuestInformations[] guests)
            : base(partyId) {
            this.partyType = partyType;
            this.partyName = partyName;
            this.fromId = fromId;
            this.fromName = fromName;
            this.leaderId = leaderId;
            this.members = members;
            this.guests = guests;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.partyType);
            writer.WriteUTF(this.partyName);
            writer.WriteVarUhLong(this.fromId);
            writer.WriteUTF(this.fromName);
            writer.WriteVarUhLong(this.leaderId);
            writer.WriteUShort((ushort) this.members.Length);
            foreach (var entry in this.members) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.guests.Length);
            foreach (var entry in this.guests) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.partyType = reader.ReadSByte();

            if (this.partyType < 0)
                throw new Exception("Forbidden value on partyType = " + this.partyType + ", it doesn't respect the following condition : partyType < 0");
            this.partyName = reader.ReadUTF();
            this.fromId = reader.ReadVarUhLong();

            if (this.fromId < 0 || this.fromId > 9007199254740990)
                throw new Exception("Forbidden value on fromId = " + this.fromId + ", it doesn't respect the following condition : fromId < 0 || fromId > 9007199254740990");
            this.fromName = reader.ReadUTF();
            this.leaderId = reader.ReadVarUhLong();

            if (this.leaderId < 0 || this.leaderId > 9007199254740990)
                throw new Exception("Forbidden value on leaderId = " + this.leaderId + ", it doesn't respect the following condition : leaderId < 0 || leaderId > 9007199254740990");
            var limit = reader.ReadUShort();
            this.members = new PartyInvitationMemberInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.members[i] = new PartyInvitationMemberInformations();
                this.members[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.guests = new PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guests[i] = new PartyGuestInformations();
                this.guests[i].Deserialize(reader);
            }
        }
    }
}