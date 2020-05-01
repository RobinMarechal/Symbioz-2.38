using System.Collections.Generic;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils.Mines {
    public class MineMap {
        public int MapId;
        public List<int> ElementIds = new List<int>();

        public MineMap(int mapId) {
            this.MapId = mapId;
        }

        public void AddElement(int elementId) {
            this.ElementIds.Add(elementId);
        }
    }
}