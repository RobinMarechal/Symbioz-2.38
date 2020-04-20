using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightShowFighterMessage : Message {
        public const ushort Id = 5864;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameFightFighterInformations informations;


        public GameFightShowFighterMessage() { }

        public GameFightShowFighterMessage(GameFightFighterInformations informations) {
            this.informations = informations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.informations.TypeId);
            this.informations.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.informations = ProtocolTypeManager.GetInstance<GameFightFighterInformations>(reader.ReadShort());
            this.informations.Deserialize(reader);
        }
    }
}