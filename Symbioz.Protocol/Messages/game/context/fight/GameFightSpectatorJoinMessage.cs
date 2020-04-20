using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightSpectatorJoinMessage : GameFightJoinMessage {
        public const ushort Id = 6504;

        public override ushort MessageId {
            get { return Id; }
        }

        public NamedPartyTeam[] namedPartyTeams;


        public GameFightSpectatorJoinMessage() { }

        public GameFightSpectatorJoinMessage(bool isTeamPhase, bool canBeCancelled, bool canSayReady, bool isFightStarted, short timeMaxBeforeFightStart, sbyte fightType, NamedPartyTeam[] namedPartyTeams)
            : base(isTeamPhase, canBeCancelled, canSayReady, isFightStarted, timeMaxBeforeFightStart, fightType) {
            this.namedPartyTeams = namedPartyTeams;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.namedPartyTeams.Length);
            foreach (var entry in this.namedPartyTeams) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.namedPartyTeams = new NamedPartyTeam[limit];
            for (int i = 0; i < limit; i++) {
                this.namedPartyTeams[i] = new NamedPartyTeam();
                this.namedPartyTeams[i].Deserialize(reader);
            }
        }
    }
}