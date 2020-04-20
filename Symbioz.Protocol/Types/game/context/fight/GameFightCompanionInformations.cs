// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightCompanionInformations : GameFightFighterInformations {
        public const short Id = 450;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte companionGenericId;
        public byte level;
        public double masterId;


        public GameFightCompanionInformations() { }

        public GameFightCompanionInformations(double contextualId,
                                              EntityLook look,
                                              EntityDispositionInformations disposition,
                                              sbyte teamId,
                                              sbyte wave,
                                              bool alive,
                                              GameFightMinimalStats stats,
                                              ushort[] previousPositions,
                                              sbyte companionGenericId,
                                              byte level,
                                              double masterId)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions) {
            this.companionGenericId = companionGenericId;
            this.level = level;
            this.masterId = masterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.companionGenericId);
            writer.WriteByte(this.level);
            writer.WriteDouble(this.masterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.companionGenericId = reader.ReadSByte();

            if (this.companionGenericId < 0)
                throw new Exception("Forbidden value on companionGenericId = " + this.companionGenericId + ", it doesn't respect the following condition : companionGenericId < 0");
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 255)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 255");
            this.masterId = reader.ReadDouble();

            if (this.masterId < -9007199254740990 || this.masterId > 9007199254740990)
                throw new Exception("Forbidden value on masterId = " + this.masterId + ", it doesn't respect the following condition : masterId < -9007199254740990 || masterId > 9007199254740990");
        }
    }
}