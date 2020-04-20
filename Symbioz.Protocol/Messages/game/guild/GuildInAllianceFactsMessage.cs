using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInAllianceFactsMessage : GuildFactsMessage {
        public const ushort Id = 6422;

        public override ushort MessageId {
            get { return Id; }
        }

        public BasicNamedAllianceInformations allianceInfos;


        public GuildInAllianceFactsMessage() { }

        public GuildInAllianceFactsMessage(GuildFactSheetInformations infos,
                                           int creationDate,
                                           ushort nbTaxCollectors,
                                           bool enabled,
                                           CharacterMinimalInformations[] members,
                                           BasicNamedAllianceInformations allianceInfos)
            : base(infos, creationDate, nbTaxCollectors, enabled, members) {
            this.allianceInfos = allianceInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.allianceInfos.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.allianceInfos = new BasicNamedAllianceInformations();
            this.allianceInfos.Deserialize(reader);
        }
    }
}