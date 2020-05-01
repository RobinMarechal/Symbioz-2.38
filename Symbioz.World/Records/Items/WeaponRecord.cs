using System.Collections.Generic;
using Symbioz.ORM;

namespace Symbioz.World.Records.Items {
    [Table("Weapons", true, 7)]
    public class WeaponRecord : ITable {
        public static List<WeaponRecord> Weapons = new List<WeaponRecord>();

        [Primary]
        public ushort Id;

        [Ignore]
        public ushort ItemId => this.Id;

        public short CraftXpRatio;

        public short MaxRange;

        public sbyte CriticalHitBonus;

        public short MinRange;

        public short MaxCastPerTurn;

        public bool Etheral;

        [Ignore]
        public ItemRecord Template;

        public bool Exchangeable;

        public bool CastTestLos;

        public sbyte CriticalHitProbability;

        public bool TwoHanded;

        public bool CastInDiagonal;

        public short ApCost;

        public bool CastInLine;

        public WeaponRecord(ushort id,
                            short craftXpRatio,
                            short maxRange,
                            sbyte criticalHitBonus,
                            short minRange,
                            short maxCastPerTurn,
                            bool etheral,
                            bool exchangeable,
                            bool castTestLos,
                            sbyte criticalHitProbability,
                            bool twoHanded,
                            bool castInDiagonal,
                            short apCost,
                            bool castInLine) {
            this.Id = id;
            this.CraftXpRatio = craftXpRatio;
            this.MaxRange = maxRange;
            this.CriticalHitBonus = criticalHitBonus;
            this.MinRange = minRange;
            this.MaxCastPerTurn = maxCastPerTurn;
            this.Etheral = etheral;
            this.Exchangeable = exchangeable;
            this.CastTestLos = castTestLos;
            this.CriticalHitProbability = criticalHitProbability;
            this.TwoHanded = twoHanded;
            this.CastInDiagonal = castInDiagonal;
            this.ApCost = apCost;
            this.CastInLine = castInLine;
            this.Template = this.ToItemRecord();
        }

        private ItemRecord ToItemRecord() {
            return ItemRecord.GetItem(this.Id);
        }

        public static WeaponRecord GetWeapon(ushort id) {
            return Weapons.Find(x => x.Id == id);
        }
    }
}