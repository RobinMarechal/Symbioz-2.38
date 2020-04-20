// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightFighterTaxCollectorLightInformations : GameFightFighterLightInformations {
        public const short Id = 457;
        public override short TypeId => Id;

        public ushort firstNameId;
        public ushort lastNameId;


        public GameFightFighterTaxCollectorLightInformations() { }

        public GameFightFighterTaxCollectorLightInformations(double id, sbyte wave, ushort level, sbyte breed, ushort firstNameId, ushort lastNameId)
            : base(id, wave, level, breed) {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.firstNameId);
            writer.WriteVarUhShort(this.lastNameId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.firstNameId = reader.ReadVarUhShort();

            if (this.firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + this.firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            this.lastNameId = reader.ReadVarUhShort();

            if (this.lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + this.lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
        }
    }
}