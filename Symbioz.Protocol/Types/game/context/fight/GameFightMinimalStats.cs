// Generated on 04/27/2016 01:13:12

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameFightMinimalStats {
        public const short Id = 31;

        public virtual short TypeId {
            get { return Id; }
        }

        public uint lifePoints;
        public uint maxLifePoints;
        public uint baseMaxLifePoints;
        public uint permanentDamagePercent;
        public uint shieldPoints;
        public short actionPoints;
        public short maxActionPoints;
        public short movementPoints;
        public short maxMovementPoints;
        public double summoner;
        public bool summoned;
        public short neutralElementResistPercent;
        public short earthElementResistPercent;
        public short waterElementResistPercent;
        public short airElementResistPercent;
        public short fireElementResistPercent;
        public short neutralElementReduction;
        public short earthElementReduction;
        public short waterElementReduction;
        public short airElementReduction;
        public short fireElementReduction;
        public short criticalDamageFixedResist;
        public short pushDamageFixedResist;
        public short pvpNeutralElementResistPercent;
        public short pvpEarthElementResistPercent;
        public short pvpWaterElementResistPercent;
        public short pvpAirElementResistPercent;
        public short pvpFireElementResistPercent;
        public short pvpNeutralElementReduction;
        public short pvpEarthElementReduction;
        public short pvpWaterElementReduction;
        public short pvpAirElementReduction;
        public short pvpFireElementReduction;
        public ushort dodgePALostProbability;
        public ushort dodgePMLostProbability;
        public short tackleBlock;
        public short tackleEvade;
        public short fixedDamageReflection;
        public sbyte invisibilityState;


        public GameFightMinimalStats() { }

        public GameFightMinimalStats(uint lifePoints,
                                     uint maxLifePoints,
                                     uint baseMaxLifePoints,
                                     uint permanentDamagePercent,
                                     uint shieldPoints,
                                     short actionPoints,
                                     short maxActionPoints,
                                     short movementPoints,
                                     short maxMovementPoints,
                                     double summoner,
                                     bool summoned,
                                     short neutralElementResistPercent,
                                     short earthElementResistPercent,
                                     short waterElementResistPercent,
                                     short airElementResistPercent,
                                     short fireElementResistPercent,
                                     short neutralElementReduction,
                                     short earthElementReduction,
                                     short waterElementReduction,
                                     short airElementReduction,
                                     short fireElementReduction,
                                     short criticalDamageFixedResist,
                                     short pushDamageFixedResist,
                                     short pvpNeutralElementResistPercent,
                                     short pvpEarthElementResistPercent,
                                     short pvpWaterElementResistPercent,
                                     short pvpAirElementResistPercent,
                                     short pvpFireElementResistPercent,
                                     short pvpNeutralElementReduction,
                                     short pvpEarthElementReduction,
                                     short pvpWaterElementReduction,
                                     short pvpAirElementReduction,
                                     short pvpFireElementReduction,
                                     ushort dodgePALostProbability,
                                     ushort dodgePMLostProbability,
                                     short tackleBlock,
                                     short tackleEvade,
                                     short fixedDamageReflection,
                                     sbyte invisibilityState) {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.baseMaxLifePoints = baseMaxLifePoints;
            this.permanentDamagePercent = permanentDamagePercent;
            this.shieldPoints = shieldPoints;
            this.actionPoints = actionPoints;
            this.maxActionPoints = maxActionPoints;
            this.movementPoints = movementPoints;
            this.maxMovementPoints = maxMovementPoints;
            this.summoner = summoner;
            this.summoned = summoned;
            this.neutralElementResistPercent = neutralElementResistPercent;
            this.earthElementResistPercent = earthElementResistPercent;
            this.waterElementResistPercent = waterElementResistPercent;
            this.airElementResistPercent = airElementResistPercent;
            this.fireElementResistPercent = fireElementResistPercent;
            this.neutralElementReduction = neutralElementReduction;
            this.earthElementReduction = earthElementReduction;
            this.waterElementReduction = waterElementReduction;
            this.airElementReduction = airElementReduction;
            this.fireElementReduction = fireElementReduction;
            this.criticalDamageFixedResist = criticalDamageFixedResist;
            this.pushDamageFixedResist = pushDamageFixedResist;
            this.pvpNeutralElementResistPercent = pvpNeutralElementResistPercent;
            this.pvpEarthElementResistPercent = pvpEarthElementResistPercent;
            this.pvpWaterElementResistPercent = pvpWaterElementResistPercent;
            this.pvpAirElementResistPercent = pvpAirElementResistPercent;
            this.pvpFireElementResistPercent = pvpFireElementResistPercent;
            this.pvpNeutralElementReduction = pvpNeutralElementReduction;
            this.pvpEarthElementReduction = pvpEarthElementReduction;
            this.pvpWaterElementReduction = pvpWaterElementReduction;
            this.pvpAirElementReduction = pvpAirElementReduction;
            this.pvpFireElementReduction = pvpFireElementReduction;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.fixedDamageReflection = fixedDamageReflection;
            this.invisibilityState = invisibilityState;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.lifePoints);
            writer.WriteVarUhInt(this.maxLifePoints);
            writer.WriteVarUhInt(this.baseMaxLifePoints);
            writer.WriteVarUhInt(this.permanentDamagePercent);
            writer.WriteVarUhInt(this.shieldPoints);
            writer.WriteVarShort(this.actionPoints);
            writer.WriteVarShort(this.maxActionPoints);
            writer.WriteVarShort(this.movementPoints);
            writer.WriteVarShort(this.maxMovementPoints);
            writer.WriteDouble(this.summoner);
            writer.WriteBoolean(this.summoned);
            writer.WriteVarShort(this.neutralElementResistPercent);
            writer.WriteVarShort(this.earthElementResistPercent);
            writer.WriteVarShort(this.waterElementResistPercent);
            writer.WriteVarShort(this.airElementResistPercent);
            writer.WriteVarShort(this.fireElementResistPercent);
            writer.WriteVarShort(this.neutralElementReduction);
            writer.WriteVarShort(this.earthElementReduction);
            writer.WriteVarShort(this.waterElementReduction);
            writer.WriteVarShort(this.airElementReduction);
            writer.WriteVarShort(this.fireElementReduction);
            writer.WriteVarShort(this.criticalDamageFixedResist);
            writer.WriteVarShort(this.pushDamageFixedResist);
            writer.WriteVarShort(this.pvpNeutralElementResistPercent);
            writer.WriteVarShort(this.pvpEarthElementResistPercent);
            writer.WriteVarShort(this.pvpWaterElementResistPercent);
            writer.WriteVarShort(this.pvpAirElementResistPercent);
            writer.WriteVarShort(this.pvpFireElementResistPercent);
            writer.WriteVarShort(this.pvpNeutralElementReduction);
            writer.WriteVarShort(this.pvpEarthElementReduction);
            writer.WriteVarShort(this.pvpWaterElementReduction);
            writer.WriteVarShort(this.pvpAirElementReduction);
            writer.WriteVarShort(this.pvpFireElementReduction);
            writer.WriteVarUhShort(this.dodgePALostProbability);
            writer.WriteVarUhShort(this.dodgePMLostProbability);
            writer.WriteVarShort(this.tackleBlock);
            writer.WriteVarShort(this.tackleEvade);
            writer.WriteVarShort(this.fixedDamageReflection);
            writer.WriteSByte(this.invisibilityState);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.lifePoints = reader.ReadVarUhInt();

            if (this.lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + this.lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            this.maxLifePoints = reader.ReadVarUhInt();

            if (this.maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + this.maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            this.baseMaxLifePoints = reader.ReadVarUhInt();

            if (this.baseMaxLifePoints < 0)
                throw new Exception("Forbidden value on baseMaxLifePoints = " + this.baseMaxLifePoints + ", it doesn't respect the following condition : baseMaxLifePoints < 0");
            this.permanentDamagePercent = reader.ReadVarUhInt();

            if (this.permanentDamagePercent < 0)
                throw new Exception("Forbidden value on permanentDamagePercent = " + this.permanentDamagePercent + ", it doesn't respect the following condition : permanentDamagePercent < 0");
            this.shieldPoints = reader.ReadVarUhInt();

            if (this.shieldPoints < 0)
                throw new Exception("Forbidden value on shieldPoints = " + this.shieldPoints + ", it doesn't respect the following condition : shieldPoints < 0");
            this.actionPoints = reader.ReadVarShort();
            this.maxActionPoints = reader.ReadVarShort();
            this.movementPoints = reader.ReadVarShort();
            this.maxMovementPoints = reader.ReadVarShort();
            this.summoner = reader.ReadDouble();

            if (this.summoner < -9007199254740990 || this.summoner > 9007199254740990)
                throw new Exception("Forbidden value on summoner = " + this.summoner + ", it doesn't respect the following condition : summoner < -9007199254740990 || summoner > 9007199254740990");
            this.summoned = reader.ReadBoolean();
            this.neutralElementResistPercent = reader.ReadVarShort();
            this.earthElementResistPercent = reader.ReadVarShort();
            this.waterElementResistPercent = reader.ReadVarShort();
            this.airElementResistPercent = reader.ReadVarShort();
            this.fireElementResistPercent = reader.ReadVarShort();
            this.neutralElementReduction = reader.ReadVarShort();
            this.earthElementReduction = reader.ReadVarShort();
            this.waterElementReduction = reader.ReadVarShort();
            this.airElementReduction = reader.ReadVarShort();
            this.fireElementReduction = reader.ReadVarShort();
            this.criticalDamageFixedResist = reader.ReadVarShort();
            this.pushDamageFixedResist = reader.ReadVarShort();
            this.pvpNeutralElementResistPercent = reader.ReadVarShort();
            this.pvpEarthElementResistPercent = reader.ReadVarShort();
            this.pvpWaterElementResistPercent = reader.ReadVarShort();
            this.pvpAirElementResistPercent = reader.ReadVarShort();
            this.pvpFireElementResistPercent = reader.ReadVarShort();
            this.pvpNeutralElementReduction = reader.ReadVarShort();
            this.pvpEarthElementReduction = reader.ReadVarShort();
            this.pvpWaterElementReduction = reader.ReadVarShort();
            this.pvpAirElementReduction = reader.ReadVarShort();
            this.pvpFireElementReduction = reader.ReadVarShort();
            this.dodgePALostProbability = reader.ReadVarUhShort();

            if (this.dodgePALostProbability < 0)
                throw new Exception("Forbidden value on dodgePALostProbability = " + this.dodgePALostProbability + ", it doesn't respect the following condition : dodgePALostProbability < 0");
            this.dodgePMLostProbability = reader.ReadVarUhShort();

            if (this.dodgePMLostProbability < 0)
                throw new Exception("Forbidden value on dodgePMLostProbability = " + this.dodgePMLostProbability + ", it doesn't respect the following condition : dodgePMLostProbability < 0");
            this.tackleBlock = reader.ReadVarShort();
            this.tackleEvade = reader.ReadVarShort();
            this.fixedDamageReflection = reader.ReadVarShort();
            this.invisibilityState = reader.ReadSByte();

            if (this.invisibilityState < 0)
                throw new Exception("Forbidden value on invisibilityState = " + this.invisibilityState + ", it doesn't respect the following condition : invisibilityState < 0");
        }
    }
}