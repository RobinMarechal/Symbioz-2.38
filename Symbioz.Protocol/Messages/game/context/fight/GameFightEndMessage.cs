using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightEndMessage : Message {
        public const ushort Id = 720;

        public override ushort MessageId {
            get { return Id; }
        }

        public int duration;
        public short ageBonus;
        public short lootShareLimitMalus;
        public FightResultListEntry[] results;
        public NamedPartyTeamWithOutcome[] namedPartyTeamsOutcomes;


        public GameFightEndMessage() { }

        public GameFightEndMessage(int duration, short ageBonus, short lootShareLimitMalus, FightResultListEntry[] results, NamedPartyTeamWithOutcome[] namedPartyTeamsOutcomes) {
            this.duration = duration;
            this.ageBonus = ageBonus;
            this.lootShareLimitMalus = lootShareLimitMalus;
            this.results = results;
            this.namedPartyTeamsOutcomes = namedPartyTeamsOutcomes;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.duration);
            writer.WriteShort(this.ageBonus);
            writer.WriteShort(this.lootShareLimitMalus);
            writer.WriteUShort((ushort) this.results.Length);
            foreach (var entry in this.results) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.namedPartyTeamsOutcomes.Length);
            foreach (var entry in this.namedPartyTeamsOutcomes) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.duration = reader.ReadInt();

            if (this.duration < 0)
                throw new Exception("Forbidden value on duration = " + this.duration + ", it doesn't respect the following condition : duration < 0");
            this.ageBonus = reader.ReadShort();
            this.lootShareLimitMalus = reader.ReadShort();
            var limit = reader.ReadUShort();
            this.results = new FightResultListEntry[limit];
            for (int i = 0; i < limit; i++) {
                this.results[i] = ProtocolTypeManager.GetInstance<FightResultListEntry>(reader.ReadShort());
                this.results[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.namedPartyTeamsOutcomes = new NamedPartyTeamWithOutcome[limit];
            for (int i = 0; i < limit; i++) {
                this.namedPartyTeamsOutcomes[i] = new NamedPartyTeamWithOutcome();
                this.namedPartyTeamsOutcomes[i].Deserialize(reader);
            }
        }
    }
}