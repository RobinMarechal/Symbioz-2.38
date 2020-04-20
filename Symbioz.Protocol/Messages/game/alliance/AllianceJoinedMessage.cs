using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceJoinedMessage : Message {
        public const ushort Id = 6402;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceInformations allianceInfo;
        public bool enabled;


        public AllianceJoinedMessage() { }

        public AllianceJoinedMessage(AllianceInformations allianceInfo, bool enabled) {
            this.allianceInfo = allianceInfo;
            this.enabled = enabled;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.allianceInfo.Serialize(writer);
            writer.WriteBoolean(this.enabled);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceInfo = new AllianceInformations();
            this.allianceInfo.Deserialize(reader);
            this.enabled = reader.ReadBoolean();
        }
    }
}