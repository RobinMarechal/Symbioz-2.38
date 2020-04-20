using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterStatsListMessage : Message {
        public const ushort Id = 500;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterCharacteristicsInformations stats;


        public CharacterStatsListMessage() { }

        public CharacterStatsListMessage(CharacterCharacteristicsInformations stats) {
            this.stats = stats;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.stats.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.stats = new CharacterCharacteristicsInformations();
            this.stats.Deserialize(reader);
        }
    }
}