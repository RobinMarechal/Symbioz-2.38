using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapRunningFightDetailsMessage : Message {
        public const ushort Id = 5751;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public GameFightFighterLightInformations[] attackers;
        public GameFightFighterLightInformations[] defenders;


        public MapRunningFightDetailsMessage() { }

        public MapRunningFightDetailsMessage(int fightId, GameFightFighterLightInformations[] attackers, GameFightFighterLightInformations[] defenders) {
            this.fightId = fightId;
            this.attackers = attackers;
            this.defenders = defenders;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteUShort((ushort) this.attackers.Length);
            foreach (var entry in this.attackers) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.defenders.Length);
            foreach (var entry in this.defenders) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            this.attackers = new GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.attackers[i] = ProtocolTypeManager.GetInstance<GameFightFighterLightInformations>(reader.ReadShort());
                this.attackers[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.defenders = new GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.defenders[i] = ProtocolTypeManager.GetInstance<GameFightFighterLightInformations>(reader.ReadShort());
                this.defenders[i].Deserialize(reader);
            }
        }
    }
}