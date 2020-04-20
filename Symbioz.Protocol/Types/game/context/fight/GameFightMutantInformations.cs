// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightMutantInformations : GameFightFighterNamedInformations {
        public const short Id = 50;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte powerLevel;


        public GameFightMutantInformations() { }

        public GameFightMutantInformations(double contextualId,
                                           EntityLook look,
                                           EntityDispositionInformations disposition,
                                           sbyte teamId,
                                           sbyte wave,
                                           bool alive,
                                           GameFightMinimalStats stats,
                                           ushort[] previousPositions,
                                           string name,
                                           PlayerStatus status,
                                           sbyte powerLevel)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions, name, status) {
            this.powerLevel = powerLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.powerLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.powerLevel = reader.ReadSByte();

            if (this.powerLevel < 0)
                throw new Exception("Forbidden value on powerLevel = " + this.powerLevel + ", it doesn't respect the following condition : powerLevel < 0");
        }
    }
}