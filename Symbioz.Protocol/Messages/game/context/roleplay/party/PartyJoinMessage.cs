using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyJoinMessage : AbstractPartyMessage {
        public const ushort Id = 5576;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte partyType;
        public ulong partyLeaderId;
        public sbyte maxParticipants;
        public PartyMemberInformations[] members;
        public PartyGuestInformations[] guests;
        public bool restricted;
        public string partyName;


        public PartyJoinMessage() { }

        public PartyJoinMessage(uint partyId,
                                sbyte partyType,
                                ulong partyLeaderId,
                                sbyte maxParticipants,
                                PartyMemberInformations[] members,
                                PartyGuestInformations[] guests,
                                bool restricted,
                                string partyName)
            : base(partyId) {
            this.partyType = partyType;
            this.partyLeaderId = partyLeaderId;
            this.maxParticipants = maxParticipants;
            this.members = members;
            this.guests = guests;
            this.restricted = restricted;
            this.partyName = partyName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.partyType);
            writer.WriteVarUhLong(this.partyLeaderId);
            writer.WriteSByte(this.maxParticipants);
            writer.WriteUShort((ushort) this.members.Length);
            foreach (var entry in this.members) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.guests.Length);
            foreach (var entry in this.guests) {
                entry.Serialize(writer);
            }

            writer.WriteBoolean(this.restricted);
            writer.WriteUTF(this.partyName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.partyType = reader.ReadSByte();

            if (this.partyType < 0)
                throw new Exception("Forbidden value on partyType = " + this.partyType + ", it doesn't respect the following condition : partyType < 0");
            this.partyLeaderId = reader.ReadVarUhLong();

            if (this.partyLeaderId < 0 || this.partyLeaderId > 9007199254740990)
                throw new Exception("Forbidden value on partyLeaderId = " + this.partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0 || partyLeaderId > 9007199254740990");
            this.maxParticipants = reader.ReadSByte();

            if (this.maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + this.maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            var limit = reader.ReadUShort();
            this.members = new PartyMemberInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.members[i] = ProtocolTypeManager.GetInstance<PartyMemberInformations>(reader.ReadShort());
                this.members[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.guests = new PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guests[i] = new PartyGuestInformations();
                this.guests[i].Deserialize(reader);
            }

            this.restricted = reader.ReadBoolean();
            this.partyName = reader.ReadUTF();
        }
    }
}