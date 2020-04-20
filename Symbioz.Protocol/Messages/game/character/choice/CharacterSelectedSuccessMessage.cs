using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterSelectedSuccessMessage : Message {
        public const ushort Id = 153;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterBaseInformations infos;
        public bool isCollectingStats;


        public CharacterSelectedSuccessMessage() { }

        public CharacterSelectedSuccessMessage(CharacterBaseInformations infos, bool isCollectingStats) {
            this.infos = infos;
            this.isCollectingStats = isCollectingStats;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.infos.Serialize(writer);
            writer.WriteBoolean(this.isCollectingStats);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.infos = new CharacterBaseInformations();
            this.infos.Deserialize(reader);
            this.isCollectingStats = reader.ReadBoolean();
        }
    }
}