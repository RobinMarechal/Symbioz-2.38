// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightTaxCollectorInformations : GameFightAIInformations {
        public const short Id = 48;

        public override short TypeId {
            get { return Id; }
        }

        public ushort firstNameId;
        public ushort lastNameId;
        public byte level;


        public GameFightTaxCollectorInformations() { }

        public GameFightTaxCollectorInformations(double contextualId,
                                                 EntityLook look,
                                                 EntityDispositionInformations disposition,
                                                 sbyte teamId,
                                                 sbyte wave,
                                                 bool alive,
                                                 GameFightMinimalStats stats,
                                                 ushort[] previousPositions,
                                                 ushort firstNameId,
                                                 ushort lastNameId,
                                                 byte level)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions) {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.level = level;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.firstNameId);
            writer.WriteVarUhShort(this.lastNameId);
            writer.WriteByte(this.level);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.firstNameId = reader.ReadVarUhShort();

            if (this.firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + this.firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            this.lastNameId = reader.ReadVarUhShort();

            if (this.lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + this.lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 255)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 255");
        }
    }
}