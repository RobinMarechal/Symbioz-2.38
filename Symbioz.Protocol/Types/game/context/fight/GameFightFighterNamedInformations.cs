// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightFighterNamedInformations : GameFightFighterInformations {
        public const short Id = 158;

        public override short TypeId {
            get { return Id; }
        }

        public string name;
        public PlayerStatus status;


        public GameFightFighterNamedInformations() { }

        public GameFightFighterNamedInformations(double contextualId,
                                                 EntityLook look,
                                                 EntityDispositionInformations disposition,
                                                 sbyte teamId,
                                                 sbyte wave,
                                                 bool alive,
                                                 GameFightMinimalStats stats,
                                                 ushort[] previousPositions,
                                                 string name,
                                                 PlayerStatus status)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions) {
            this.name = name;
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUTF(this.name);
            this.status.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.name = reader.ReadUTF();
            this.status = new PlayerStatus();
            this.status.Deserialize(reader);
        }
    }
}