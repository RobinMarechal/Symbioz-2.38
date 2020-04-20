using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyInvitationMessage : AbstractPartyMessage {
        public const ushort Id = 5586;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte partyType;
        public string partyName;
        public sbyte maxParticipants;
        public ulong fromId;
        public string fromName;
        public ulong toId;


        public PartyInvitationMessage() { }

        public PartyInvitationMessage(uint partyId, sbyte partyType, string partyName, sbyte maxParticipants, ulong fromId, string fromName, ulong toId)
            : base(partyId) {
            this.partyType = partyType;
            this.partyName = partyName;
            this.maxParticipants = maxParticipants;
            this.fromId = fromId;
            this.fromName = fromName;
            this.toId = toId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.partyType);
            writer.WriteUTF(this.partyName);
            writer.WriteSByte(this.maxParticipants);
            writer.WriteVarUhLong(this.fromId);
            writer.WriteUTF(this.fromName);
            writer.WriteVarUhLong(this.toId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.partyType = reader.ReadSByte();

            if (this.partyType < 0)
                throw new Exception("Forbidden value on partyType = " + this.partyType + ", it doesn't respect the following condition : partyType < 0");
            this.partyName = reader.ReadUTF();
            this.maxParticipants = reader.ReadSByte();

            if (this.maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + this.maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            this.fromId = reader.ReadVarUhLong();

            if (this.fromId < 0 || this.fromId > 9007199254740990)
                throw new Exception("Forbidden value on fromId = " + this.fromId + ", it doesn't respect the following condition : fromId < 0 || fromId > 9007199254740990");
            this.fromName = reader.ReadUTF();
            this.toId = reader.ReadVarUhLong();

            if (this.toId < 0 || this.toId > 9007199254740990)
                throw new Exception("Forbidden value on toId = " + this.toId + ", it doesn't respect the following condition : toId < 0 || toId > 9007199254740990");
        }
    }
}