using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Symbioz.Core;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils.Mines {
    public class Mine {
        public List<MineMap> Maps = new List<MineMap>();
        public Dictionary<int, int> Ores = new Dictionary<int, int>();

        public Mine() { }

        public void AddMap(ref MineMap map) {
            this.Maps.Add(map);
        }

        public int AddOre(int oreId, int quantity) {
            if (this.Ores.ContainsKey(oreId)) {
                quantity += this.Ores[oreId];
                this.Ores.Remove(oreId);
            }

            this.Ores.Add(oreId, quantity);
            return quantity;
        }
    }
}