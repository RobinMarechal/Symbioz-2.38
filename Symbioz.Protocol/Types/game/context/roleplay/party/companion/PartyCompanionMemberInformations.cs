// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class PartyCompanionMemberInformations : PartyCompanionBaseInformations {
        public const short Id = 452;

        public override short TypeId {
            get { return Id; }
        }

        public ushort initiative;
        public uint lifePoints;
        public uint maxLifePoints;
        public ushort prospecting;
        public byte regenRate;


        public PartyCompanionMemberInformations() { }

        public PartyCompanionMemberInformations(sbyte indexId, sbyte companionGenericId, EntityLook entityLook, ushort initiative, uint lifePoints, uint maxLifePoints, ushort prospecting, byte regenRate)
            : base(indexId, companionGenericId, entityLook) {
            this.initiative = initiative;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.initiative);
            writer.WriteVarUhInt(this.lifePoints);
            writer.WriteVarUhInt(this.maxLifePoints);
            writer.WriteVarUhShort(this.prospecting);
            writer.WriteByte(this.regenRate);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.initiative = reader.ReadVarUhShort();

            if (this.initiative < 0)
                throw new Exception("Forbidden value on initiative = " + this.initiative + ", it doesn't respect the following condition : initiative < 0");
            this.lifePoints = reader.ReadVarUhInt();

            if (this.lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + this.lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            this.maxLifePoints = reader.ReadVarUhInt();

            if (this.maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + this.maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            this.prospecting = reader.ReadVarUhShort();

            if (this.prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + this.prospecting + ", it doesn't respect the following condition : prospecting < 0");
            this.regenRate = reader.ReadByte();

            if (this.regenRate < 0 || this.regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + this.regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
        }
    }
}