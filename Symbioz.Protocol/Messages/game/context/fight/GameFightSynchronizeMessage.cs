using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightSynchronizeMessage : Message {
        public const ushort Id = 5921;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameFightFighterInformations[] fighters;


        public GameFightSynchronizeMessage() { }

        public GameFightSynchronizeMessage(GameFightFighterInformations[] fighters) {
            this.fighters = fighters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.fighters.Length);
            foreach (var entry in this.fighters) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.fighters = new GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.fighters[i] = ProtocolTypeManager.GetInstance<GameFightFighterInformations>(reader.ReadShort());
                this.fighters[i].Deserialize(reader);
            }
        }
    }
}