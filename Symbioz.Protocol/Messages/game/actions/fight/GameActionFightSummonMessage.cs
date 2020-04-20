using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightSummonMessage : AbstractGameActionMessage {
        public const ushort Id = 5825;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameFightFighterInformations[] summons;


        public GameActionFightSummonMessage() { }

        public GameActionFightSummonMessage(ushort actionId, double sourceId, GameFightFighterInformations[] summons)
            : base(actionId, sourceId) {
            this.summons = summons;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.summons.Length);
            foreach (var entry in this.summons) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.summons = new GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.summons[i] = ProtocolTypeManager.GetInstance<GameFightFighterInformations>(reader.ReadShort());
                this.summons[i].Deserialize(reader);
            }
        }
    }
}