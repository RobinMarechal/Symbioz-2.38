using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceTaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionExtendedMessage {
        public const ushort Id = 6445;

        public override ushort MessageId {
            get { return Id; }
        }

        public BasicNamedAllianceInformations alliance;


        public AllianceTaxCollectorDialogQuestionExtendedMessage() { }

        public AllianceTaxCollectorDialogQuestionExtendedMessage(BasicGuildInformations guildInfo,
                                                                 ushort maxPods,
                                                                 ushort prospecting,
                                                                 ushort wisdom,
                                                                 sbyte taxCollectorsCount,
                                                                 int taxCollectorAttack,
                                                                 uint kamas,
                                                                 ulong experience,
                                                                 uint pods,
                                                                 uint itemsValue,
                                                                 BasicNamedAllianceInformations alliance)
            : base(guildInfo, maxPods, prospecting, wisdom, taxCollectorsCount, taxCollectorAttack, kamas, experience, pods, itemsValue) {
            this.alliance = alliance;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            this.alliance.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.alliance = new BasicNamedAllianceInformations();
            this.alliance.Deserialize(reader);
        }
    }
}