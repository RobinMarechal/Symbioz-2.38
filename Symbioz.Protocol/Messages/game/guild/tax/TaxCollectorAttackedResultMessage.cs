using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorAttackedResultMessage : Message {
        public const ushort Id = 5635;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool deadOrAlive;
        public TaxCollectorBasicInformations basicInfos;
        public BasicGuildInformations guild;


        public TaxCollectorAttackedResultMessage() { }

        public TaxCollectorAttackedResultMessage(bool deadOrAlive, TaxCollectorBasicInformations basicInfos, BasicGuildInformations guild) {
            this.deadOrAlive = deadOrAlive;
            this.basicInfos = basicInfos;
            this.guild = guild;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.deadOrAlive);
            this.basicInfos.Serialize(writer);
            this.guild.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.deadOrAlive = reader.ReadBoolean();
            this.basicInfos = new TaxCollectorBasicInformations();
            this.basicInfos.Deserialize(reader);
            this.guild = new BasicGuildInformations();
            this.guild.Deserialize(reader);
        }
    }
}