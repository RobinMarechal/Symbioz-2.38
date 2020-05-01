using System.Collections.Generic;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils {

    public class LinkItem {
        public static List<LinkItem> Entries = new List<LinkItem>();
        public static List<LinkItem> Exits = new List<LinkItem>();

        internal enum EType {
            ENTRY,
            EXIT
        }

        private EType _Type;

        public int ElementId { get; }
        public int ElementType { get; }
        public ushort SkillId { get; }
        public int SpawnCellId { get; }
        public int MapId { get; }

        private LinkItem(EType type, int mapId, int elementId, int elementType, ushort skillId, int spawnCellId) {
            this._Type = type;
            this.MapId = mapId;
            this.ElementId = elementId;
            this.ElementType = elementType;
            this.SkillId = skillId;
            this.SpawnCellId = spawnCellId;
        }

        public static LinkItem InitEntry(int mapId, int elementId, int elementType, ushort skillId, int spawnCellId) {
            var linkItem = new LinkItem(EType.ENTRY, mapId, elementId, elementType, skillId, spawnCellId);
            Entries.Add(linkItem);

            return linkItem;
        }

        public static LinkItem InitExit(int mapId, int elementId, int elementType, ushort skillId, int spawnCellId) {
            var linkItem = new LinkItem(EType.EXIT, mapId, elementId, elementType, skillId, spawnCellId);
            Exits.Add(linkItem);

            return linkItem;
        }

        public override string ToString() {
            var type = this._Type == EType.ENTRY ? "Entry" : "Exit";

            return $"{type}(MapId={this.MapId}, ElementId={this.ElementId}, ElementType={this.ElementType}, SpawnCellId={this.SpawnCellId})";
        }
    }
}