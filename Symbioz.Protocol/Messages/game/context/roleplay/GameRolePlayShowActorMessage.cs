using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayShowActorMessage : Message {
        public const ushort Id = 5632;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameRolePlayActorInformations informations;


        public GameRolePlayShowActorMessage() { }

        public GameRolePlayShowActorMessage(GameRolePlayActorInformations informations) {
            this.informations = informations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.informations.TypeId);
            this.informations.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.informations = ProtocolTypeManager.GetInstance<GameRolePlayActorInformations>(reader.ReadShort());
            this.informations.Deserialize(reader);
        }
    }
}