using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FighterStatsListMessage : Message {
        public const ushort Id = 6322;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterCharacteristicsInformations stats;


        public FighterStatsListMessage() { }

        public FighterStatsListMessage(CharacterCharacteristicsInformations stats) {
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