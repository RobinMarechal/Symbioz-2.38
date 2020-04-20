// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightFighterCompanionLightInformations : GameFightFighterLightInformations {
        public const short Id = 454;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte companionId;
        public double masterId;


        public GameFightFighterCompanionLightInformations() { }

        public GameFightFighterCompanionLightInformations(double id, sbyte wave, ushort level, sbyte breed, sbyte companionId, double masterId)
            : base(id, wave, level, breed) {
            this.companionId = companionId;
            this.masterId = masterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.companionId);
            writer.WriteDouble(this.masterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.companionId = reader.ReadSByte();

            if (this.companionId < 0)
                throw new Exception("Forbidden value on companionId = " + this.companionId + ", it doesn't respect the following condition : companionId < 0");
            this.masterId = reader.ReadDouble();

            if (this.masterId < -9007199254740990 || this.masterId > 9007199254740990)
                throw new Exception("Forbidden value on masterId = " + this.masterId + ", it doesn't respect the following condition : masterId < -9007199254740990 || masterId > 9007199254740990");
        }
    }
}