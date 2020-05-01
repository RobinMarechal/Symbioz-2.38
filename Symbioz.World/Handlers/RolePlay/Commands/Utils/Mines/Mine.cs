using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Symbioz.Core;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils.Mines {
    public class Mine {
        public List<MineMap> Maps = new List<MineMap>();
        public Dictionary<int, int> ores = new Dictionary<int, int>();

        public Mine() { }

        public void AddMap(ref MineMap map) {
            this.Maps.Add(map);
        }

        public void AddOre(int oreId, int quantity) {
            if (this.ores.ContainsKey(oreId)) {
                quantity += this.ores[oreId];
                this.ores.Remove(oreId);
            }

            this.ores.Add(oreId, quantity);
        }
    }
}