using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceInvitedMessage : Message {
        public const ushort Id = 6397;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong recruterId;
        public string recruterName;
        public BasicNamedAllianceInformations allianceInfo;


        public AllianceInvitedMessage() { }

        public AllianceInvitedMessage(ulong recruterId, string recruterName, BasicNamedAllianceInformations allianceInfo) {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.allianceInfo = allianceInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.recruterId);
            writer.WriteUTF(this.recruterName);
            this.allianceInfo.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.recruterId = reader.ReadVarUhLong();

            if (this.recruterId < 0 || this.recruterId > 9007199254740990)
                throw new Exception("Forbidden value on recruterId = " + this.recruterId + ", it doesn't respect the following condition : recruterId < 0 || recruterId > 9007199254740990");
            this.recruterName = reader.ReadUTF();
            this.allianceInfo = new BasicNamedAllianceInformations();
            this.allianceInfo.Deserialize(reader);
        }
    }
}