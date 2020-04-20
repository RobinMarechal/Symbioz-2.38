using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInvitationStateRecruterMessage : Message {
        public const ushort Id = 5563;

        public override ushort MessageId {
            get { return Id; }
        }

        public string recrutedName;
        public sbyte invitationState;


        public GuildInvitationStateRecruterMessage() { }

        public GuildInvitationStateRecruterMessage(string recrutedName, sbyte invitationState) {
            this.recrutedName = recrutedName;
            this.invitationState = invitationState;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.recrutedName);
            writer.WriteSByte(this.invitationState);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.recrutedName = reader.ReadUTF();
            this.invitationState = reader.ReadSByte();

            if (this.invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + this.invitationState + ", it doesn't respect the following condition : invitationState < 0");
        }
    }
}